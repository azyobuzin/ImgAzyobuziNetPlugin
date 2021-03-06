﻿using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Acuerdo.Plugin;
using Inscribe.Core;
using Inscribe.Plugin;
using Inscribe.Storage;

namespace ImgAzyobuziNetPlugin
{
    [Export(typeof(IPlugin))]
    public class EntryPoint : IPlugin
    {
        public string Name
        {
            get
            {
                return Assembly.GetExecutingAssembly()
                    .GetCustomAttributes(false)
                    .OfType<AssemblyTitleAttribute>()
                    .Select(a => a.Title)
                    .FirstOrDefault();
            }
        }

        public Version Version
        {
            get { return Assembly.GetExecutingAssembly().GetName().Version; }
        }

        public void Loaded()
        {
            UploaderManager.RegisterResolver(new Resolver());
            KernelService.AddMenu("[img.azyobuzi.net] 正規表現を再取得", LoadRegex);
            LoadRegex();
        }

        public IConfigurator ConfigurationInterface
        {
            get { return null; }
        }

        public static void LoadRegex()
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    using (var notify = NotifyStorage.NotifyManually("img.azyobuzi.net: 正規表現を取得しています..."))
                    using (var wc = new WebClient())
                    using (var reader = JsonReaderWriterFactory.CreateJsonReader(
                        wc.OpenRead("http://img.azyobuzi.net/api/regex.json"),
                        XmlDictionaryReaderQuotas.Max))
                    {
                        Resolver.RegexArray = XElement.Load(reader).Elements()
                            .Select(xe => new Regex(xe.Element("regex").Value, RegexOptions.IgnoreCase))
                            .ToArray();
                    }
                }
                catch (Exception ex)
                {
                    ExceptionStorage.Register(
                        ex,
                        ExceptionCategory.PluginError,
                        "img.azyobuzi.net: 正規表現の読み込みに失敗しました。",
                        LoadRegex
                    );
                }
            });
        }
    }
}
