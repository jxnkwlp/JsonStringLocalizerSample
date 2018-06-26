using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using System.Reflection;
using System.IO;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Concurrent;
using System.Diagnostics;
using Microsoft.AspNetCore.Html;

namespace JsonStringLocalizerTest
{
    public class JsonStringLocalizer : IStringLocalizer
    {
        private readonly Dictionary<string, string> _all;

        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly LocalizationOptions _options;

        private readonly string _baseResourceName;
        private readonly CultureInfo _cultureInfo;

        public LocalizedString this[string name]
        {
            get
            {
                return Get(name);
            }
        }

        public LocalizedString this[string name, params object[] arguments]
        {
            get
            {
                // new  HtmlFormattableString()
                return Get(name, arguments);
            }
        }

        public JsonStringLocalizer(IHostingEnvironment hostingEnvironment, LocalizationOptions options, string baseResourceName, CultureInfo culture)
        {
            _options = options;
            _hostingEnvironment = hostingEnvironment;

            _cultureInfo = culture ?? CultureInfo.CurrentUICulture;
            _baseResourceName = baseResourceName + "." + _cultureInfo.Name;
            _all = GetAll();

        }

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            return _all.Select(t => new LocalizedString(t.Key, t.Value, true)).ToArray();
        }

        public IStringLocalizer WithCulture(CultureInfo culture)
        {
            if (culture == null)
                return this;

            CultureInfo.CurrentUICulture = culture;
            CultureInfo.DefaultThreadCurrentCulture = culture;

            return new JsonStringLocalizer(_hostingEnvironment, _options, _baseResourceName, culture);
        }


        private LocalizedString Get(string name, params object[] arguments)
        {
            if (_all.ContainsKey(name))
            {
                var current = _all[name];
                return new LocalizedString(name, string.Format(current, arguments));
            }
            return new LocalizedString(name, name, true);
        }

        private Dictionary<string, string> GetAll()
        {
            var file = Path.Combine(_hostingEnvironment.ContentRootPath, _baseResourceName + ".json");
            if (!string.IsNullOrEmpty(_options.ResourcesPath))
                file = Path.Combine(_hostingEnvironment.ContentRootPath, _options.ResourcesPath, _baseResourceName + ".json");

            Debug.WriteLineIf(File.Exists(file), "Json Resources Path find in " + file);

            Debug.WriteLineIf(!File.Exists(file), "Path not found! " + file);

            if (!File.Exists(file))
                return new Dictionary<string, string>();

            try
            {
                var txt = File.ReadAllText(file);

                return JsonConvert.DeserializeObject<Dictionary<string, string>>(txt);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            return new Dictionary<string, string>();
        }
    }

    //public struct JsonStringLocalizer<TResource> : JsonStringLocalizer, IStringLocalizer<TResource>
    //{
    //    public LocalizedString this[string name] => throw new NotImplementedException();

    //    public LocalizedString this[string name, params object[] arguments] => throw new NotImplementedException();

    //    public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public IStringLocalizer WithCulture(CultureInfo culture)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

}
