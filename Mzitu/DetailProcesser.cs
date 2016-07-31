using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace Mzitu
{
    public class DetailProcesser
    {
        /// <summary>
        /// 详细页链接
        /// </summary>
        private string _detailUrl;
        /// <summary>
        /// 当前详细页链接变化时触发此事件
        /// </summary>
        public event EventHandler<StringEventArgs> OnUrlChanged;

        public event EventHandler<StringEventArgs> OnSavePathChanged;

        public class StringEventArgs : EventArgs
        {
            public StringEventArgs(string url)
            {
                StringArg = url;
            }

            public string StringArg;
        }

        private string _savaPicPath;
        /// <summary>
        /// 保存路径
        /// </summary>
        public string SavePicPath
        {
            get { return _savaPicPath; }
            private set
            {
                if (_savaPicPath != value)
                {
                    OnSavePathChanged?.Invoke(this, new StringEventArgs(value));
                    _savaPicPath = value;
                }
            }
        }


        public string SavePath { get; private set; }
        /// <summary>
        /// 获取详细页链接
        /// </summary>
        public string DetailUrl
        {
            get { return _detailUrl; }
            private set
            {
                if (_detailUrl != value)
                {
                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        if (OnUrlChanged != null)
                        {
                            OnUrlChanged(this, new StringEventArgs(value));
                        }
                    }
                    _detailUrl = value;
                }

            }
        }

        private bool _stop = false;

        public DetailProcesser(string url, string savePath)
        {
            savePath = savePath.TrimEnd('\\');
            SavePath = savePath;
            DetailUrl = url;
        }

        /// <summary>
        /// 开始抓取
        /// </summary>
        public void Start()
        {
            new Thread(() =>
            {
                var pagehtml = GetPageHtml(DetailUrl);
                if (string.IsNullOrWhiteSpace(pagehtml))
                {

                }
                DetailUrl = GetNextPageUrl(pagehtml);
                if (string.IsNullOrWhiteSpace(DetailUrl))
                {

                }
                do
                {
                    var picUrl = GetPicUrl(pagehtml);
                    if (string.IsNullOrWhiteSpace(picUrl))
                    {

                    }
                    var title = GetPicTitle(pagehtml);
                    if (string.IsNullOrWhiteSpace(title))
                    {

                    }

                    new Thread(DownloadOnePic).Start(new[] { picUrl, title });
                    //DownloadOnePic(picUrl);
                    pagehtml = GetPageHtml(DetailUrl);
                    if (string.IsNullOrWhiteSpace(pagehtml))
                    {

                    }
                    DetailUrl = GetNextPageUrl(pagehtml);
                    if (string.IsNullOrWhiteSpace(DetailUrl))
                    {

                    }
                    if (_stop)
                    {
                        _stop = false;
                        break;
                    }

                } while (!string.IsNullOrEmpty(DetailUrl));
            }).Start();

        }

        private string GetPageHtml(string url)
        {

            for (int i = 0; i < 10; i++)
            {
                var h = HttpRequestHelper.Get_Data(url, null, "utf-8", null);
                if (string.IsNullOrWhiteSpace(h))
                {
                    Thread.Sleep(1000);
                    continue;
                }
                return h;
            }
            throw new Exception(url);
        }

        private string GetPicTitle(string pagehtml)
        {
            var res = "";

            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(pagehtml);
            var picNode = document.DocumentNode.SelectSingleNode("//h2[@class='main-title']");
            if (picNode != null)
            {
                res = picNode.InnerText;
            }
            return res;
        }

        /// <summary>
        /// 停止抓取
        /// </summary>
        public void Stop()
        {
            _stop = true;
        }
        /// <summary>
        /// 下载图片
        /// </summary>
        /// <param name="picUrl">图片url</param>
        private void DownloadOnePic(object obj)
        {
            var arr = (string[])obj;
            string picUrl = arr[0];
            string title = arr[1];
            if (!Directory.Exists(SavePath))
                Directory.CreateDirectory(SavePath);
            var s = Regex.Match(picUrl, @"(?<year>[0-9]{4})/(?<month>[0-9]{1,2})/(?<name>.+)\..+");
            var name = s.Groups["year"].Value + s.Groups["month"].Value + s.Groups["name"].Value;
            SavePicPath = string.Format("{0}\\{3}_{1}.{2}", SavePath, title, picUrl.Split('.').Last(),
              name);
            var sp = SavePicPath;
            if (File.Exists(SavePicPath))
            {
                SavePicPath += " 已存在";
                return;
            }
            SavePicPath += " 下载中...";
            var time = 3;
            GOTO_RETRY:
            var stream = HttpRequestHelper.Get_ImgStream(picUrl, null);
            Image image = null;
            try
            {
                image = Image.FromStream(stream);
            }
            catch (Exception)
            {
                SavePicPath += " 失败";
            }
            if (image != null)
            {
                try
                {
                    image.Save(sp, image.RawFormat);
                    SavePicPath += " 完成";
                }
                catch (Exception ex)
                {
                    if (time == 0)
                    {
                        SavePicPath += " 失败";
                        return;
                    }
                    SavePicPath += " 重试";
                    time--;
                    goto GOTO_RETRY;
                }
            }
        }

        private string GetPicUrl(string pagehtml)
        {
            var res = "";
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(pagehtml);
            var picNode = document.DocumentNode.SelectSingleNode("//div[@class='main-image']//img");
            if (picNode != null)
            {
                res = picNode.GetAttributeValue("src", "");
            }
            return res;
        }

        private string GetNextPageUrl(string html)
        {
            var res = "";

            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(html);
            var picNode = document.DocumentNode.SelectSingleNode("//div[@class='pagenavi']/a[last()]");
            if (picNode != null)
            {
                res = picNode.GetAttributeValue("href", "");
            }

            return res;
        }
    }
}
