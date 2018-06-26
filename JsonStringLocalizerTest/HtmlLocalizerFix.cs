using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JsonStringLocalizerTest
{
    public class HtmlLocalizerFix<TResource> : HtmlLocalizer<TResource>, IHtmlLocalizer<TResource>, IHtmlLocalizer
    {
        // private readonly IHtmlLocalizer _localizer;

        public HtmlLocalizerFix(IHtmlLocalizerFactory factory) : base(factory)
        {
        }

        public override LocalizedHtmlString this[string name]
        {
            get
            {
                return base[name];
            }
        }

        public override LocalizedHtmlString this[string name, params object[] arguments]
        {
            get
            {
                return base[name, arguments];
                // return ToHtmlString(_localizer[name, arguments]);
            }
        }

    }

    public class HtmlLocalizerFix : HtmlLocalizer, IHtmlLocalizer
    {
        public HtmlLocalizerFix(IStringLocalizer localizer) : base(localizer)
        {
        }

        public override LocalizedHtmlString this[string name]
        {
            get
            {
                return base[name];
            }
        }

        public override LocalizedHtmlString this[string name, params object[] arguments]
        {
            get
            {
                return base[name, arguments];
            }
        }
    }
}
