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
        private Dictionary<string[], Thread> _taskList = new Dictionary<string[], Thread>();

        private int max = 50;
        private int now = 0;

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
                DetailUrl = GetNextPageUrl(pagehtml);
                do
                {
                    var picUrl = GetPicUrl(pagehtml);
                    var title = GetPicTitle(pagehtml);

                    var th = new Thread(DownloadOnePic);
                    //_taskList.Add(new[] { picUrl, title }, th);
                    ListOperation(new[] { picUrl, title }, th, Op.Add);
                    //DownloadOnePic(picUrl);
                    pagehtml = GetPageHtml(DetailUrl);
                    DetailUrl = GetNextPageUrl(pagehtml);
                    if (_stop)
                    {
                        _stop = false;
                        break;
                    }

                } while (!string.IsNullOrEmpty(DetailUrl));
            })
            { IsBackground = true }.Start();

            new Thread(() =>
            {
                for (;;)
                {
                    if (_stop)
                    {
                        break;
                    }
                    int max = 50;
                    int now = 0;
                    var t = ListOperation(null, null, Op.Get);
                    if (t.Key != null)
                    {
                        if (now < max)
                        {
                            ListOperation(t.Key, null, Op.Remove);
                            //_taskList.Remove(t.Key);
                            t.Value.Start(t.Key);
                        }
                    }
                    Thread.Sleep(100);
                }

            })
            { IsBackground = true }.Start();

        }

        private enum Op
        {
            Add,
            Remove,
            Get
        }

        private object lockObj = new object();
        private KeyValuePair<string[], Thread> ListOperation(string[] ss, Thread tt, Op op)
        {
            lock (lockObj)
            {
                KeyValuePair<string[], Thread> d = new KeyValuePair<string[], Thread>();
                switch (op)
                {
                    case Op.Add:
                        _taskList.Add(ss, tt);
                        break;
                    case Op.Remove:
                        _taskList.Remove(ss);
                        break;
                    case Op.Get:
                        d = _taskList.FirstOrDefault();
                        break;
                }
                return d;
            }
        }


        private string GetPageHtml(string url)
        {
            for (int i = 0; i < 10; i++)
            {
                var h = HttpRequestHelper.Get_Data(url, null, "utf-8", null);
                if (!string.IsNullOrWhiteSpace(h))
                {
                    return h;
                }
                Thread.Sleep(1000);
            }
            //return "";
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
            now++;
            var arr = (string[])obj;
            string picUrl = arr[0];
            string title = arr[1];
            if (!Directory.Exists(SavePath))
                Directory.CreateDirectory(SavePath);
            var s = Regex.Match(picUrl, @"(?<year>[0-9]{4})/(?<month>[0-9]{1,2})/(?<name>.+)\..+");
            var name = s.Groups["year"].Value + s.Groups["month"].Value + s.Groups["name"].Value;
            SavePicPath = string.Format("{0}\\{3}_{1}.{2}", SavePath, title, picUrl.Split('.').Last(), name);
            var sp = SavePicPath;
            if (File.Exists(SavePicPath))
            {
                SavePicPath += " 已存在";
                return;
            }
            SavePicPath += " 下载中...";
            var time = 3;
            GOTO_RETRY:
            var stream = GetImgStream(picUrl);
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
            now--;
        }

        private Stream GetImgStream(string picUrl)
        {
            Stream s = null;
            for (int i = 0; i < 10; i++)
            {
                s = HttpRequestHelper.Get_ImgStream(picUrl, null);
                if (s != null)
                {
                    return s;
                }
                Thread.Sleep(1000);
            }
            throw new Exception(picUrl);
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
