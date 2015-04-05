using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using BizInfo.Harvesting.Services.Processing;
using BizInfo.Harvesting.Services.Processing.CompanyScoreTools;
using BizInfo.Model.Interfaces;

namespace BizInfo.Harvesting.Interfaces.Processing
{
    public class DispatchingWebSourceProcessor : IWebSourceProcessor
    {
        private List <Tuple<Regex, IWebSourceProcessor>> processors = new List<Tuple<Regex, IWebSourceProcessor>>();

        public void Process(IWebSource webSource, IBizInfoStorage storage, UrlLoadOptions loadOptions = UrlLoadOptions.LoadFromCacheIfPossible | UrlLoadOptions.StoreToCacheWhenLoaded)
        {
            GetProcessor(webSource).Process(webSource, storage, loadOptions);
        }

        public IWebSourceProcessor GetProcessor(IWebSource webSource)
        {
            return processors.Where(regexAndProcessor => regexAndProcessor.Item1.IsMatch(webSource.Url)).Select(regexAndProcessor => regexAndProcessor.Item2).FirstOrDefault();
        }

        public void AddProcessor(string regexString, IWebSourceProcessor processor)
        {
            AddProcessor(new Regex(regexString), processor);
        }

        public void AddProcessor(Regex regexString, IWebSourceProcessor processor)
        {
            processors.Add(new Tuple<Regex, IWebSourceProcessor>(regexString, processor));
        }

        public static DispatchingWebSourceProcessor Create()
        {
            var tagger = new Tagger();
            var companyScoreComputer = new CompanyScoreComputer();
            var hyperInzerceParser = new HyperInzerceParser();
            var bazosParser = new BazosParser();
            var annonceParser = new AnnonceParser();
            var inzertExpresParser = new InzertExpresParser();
            var sbazarParser = new SBazarParser();
            var avizoParser = new AvizoParser();
            var aukroParser = new AukroParser();
            var bezRealitkyParser = new BezRealitkyParser();
            var processor = new DispatchingWebSourceProcessor();
            var failureHandler = new ParsingFailureDetector();
            processor.AddProcessor(@"^http://(byty|nemovitosti-reality).hyperinzerce.cz/[^/]+/inzerat/[^/]+/", new HyperinzerceProcessor { Parser = hyperInzerceParser, Tagger = tagger, CompanyScoreComputer = companyScoreComputer, ParsingFailureDetector = failureHandler});
            processor.AddProcessor(@"^http://(nakladni-uzitkove-vozy|autobazar|motorky).hyperinzerce.cz/[^/]+/inzerat/[^/]+/", new HyperinzerceProcessor { Parser = hyperInzerceParser, Tagger = tagger, CompanyScoreComputer = companyScoreComputer, ParsingFailureDetector = failureHandler });
            processor.AddProcessor(@"^http://([^\.]+).bazos.cz/inzerat/[^/]+/", new BazosProcessor { Parser = bazosParser, Tagger = tagger, CompanyScoreComputer = companyScoreComputer, ParsingFailureDetector = failureHandler });
            processor.AddProcessor(@"^http://www.annonce.cz/detail/", new AnnonceProcessor { Parser = annonceParser, Tagger = tagger, CompanyScoreComputer = companyScoreComputer, ParsingFailureDetector = failureHandler });
            processor.AddProcessor(@"^http://www.inzertexpres.cz/\d+/", new InzertExpresProcessor { Parser = inzertExpresParser, Tagger = tagger, CompanyScoreComputer = companyScoreComputer, ParsingFailureDetector = failureHandler });
            processor.AddProcessor(@"^http://([^\.]+).sbazar.cz/", new SBazarProcessor { Parser = sbazarParser, Tagger = tagger, CompanyScoreComputer = companyScoreComputer, ParsingFailureDetector = failureHandler });
            processor.AddProcessor(@"^http://([^\.]+).avizo.cz/", new AvizoProcessor { Parser = avizoParser, Tagger = tagger, CompanyScoreComputer = companyScoreComputer, ParsingFailureDetector = failureHandler });
            processor.AddProcessor(@"^http://([^\.]+).bezrealitky.cz/", new BezRealitkyProcessor { Parser = bezRealitkyParser, Tagger = tagger, CompanyScoreComputer = companyScoreComputer, ParsingFailureDetector = failureHandler });
            processor.AddProcessor(@"^http://([^\.]+).aukro.cz/", new AukroProcessor { Parser = aukroParser, Tagger = tagger, CompanyScoreComputer = companyScoreComputer, ParsingFailureDetector = failureHandler });
            return processor;
        }
    }
}