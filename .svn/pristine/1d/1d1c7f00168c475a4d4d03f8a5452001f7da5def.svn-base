using System;
using System.Linq;
using BizInfo.Model.Interfaces;

namespace BizInfo.Harvesting.Services.Processing.CompanyScoreTools
{
    public class CompanyScoreComputer : ICompanyScoreComputer
    {
        private FragmentsOccurencyDictionary occurencies = new FragmentsOccurencyDictionary();
        
        public double ComputeScore(IBizInfo bizInfo)
        {
            var structured = bizInfo.StructuredContentDocument;
            if (structured == null) return 0.0;
            var authorPenalty = GetAuthorPenalty(structured.Author) + GetTextPenalty(bizInfo.Text);
            var scores = structured.Phones.Select(ph => ComputeScoreOfPhone(ph, authorPenalty)).Concat(structured.Mails.Select(mail => ComputeScoreOfMail(mail, authorPenalty))).ToArray();
            double score = 0.0;
            score = scores.Length > 0 ? scores.Max() : OccurencyToScore(authorPenalty);
            return score;
        }

        private int GetTextPenalty(string text)
        {
            var penalty = 0;
            if (text.Contains("ID nabídky")) penalty += 50;
            if (text.Contains("Kód zakázky")) penalty += 50;
            if (text.Contains("u makléøe") || text.Contains("pøímo makléøe") || text.Contains("vèetnì provize")) penalty += 20;
            if (text.Contains("provize")) penalty += 10;
            return penalty;
        }

        private int GetAuthorPenalty(string author)
        {
            if (string.IsNullOrEmpty(author)) return 0;
            author = author.ToLower();
            int penalty = 0;
            if (author.Contains("reality") || author.Contains("estate")) penalty += 20;
            if (author.Contains("realit")) penalty += 20;
            if (author.Contains("real") || author.Contains("s.r.o.") || author.Contains("a.s.") || author.Contains("spol.") || author.Contains("bazar")) penalty += 20;
            return penalty;
        }

        private double ComputeScoreOfPhone(string phone, int penalty)
        {
            if (phone.StartsWith("1")) return 0.0;
            int occurency;
            if (!occurencies.TryGetValue(phone, out occurency)) occurency = 0;
            return ComputeScoreOfPhoneByOccurency(phone, occurency + penalty);
        }

        private double ComputeScoreOfPhoneByOccurency(string phone, int occurency)
        {
            return OccurencyToScore(occurency);
        }

        private double OccurencyToScore(int occurency)
        {
            if (occurency <= 1) return 0.0;
            var score = 1 - Math.Pow(1.0/(double) occurency, 0.83);
            return score;
        }

        private double ComputeScoreOfMail(string mail, int penalty)
        {
            int occurency;
            if (!occurencies.TryGetValue(mail, out occurency)) occurency = 0;
            occurency += GetMailPenalty(mail);
            return ComputeScoreOfMailByOccurency(mail, occurency + penalty);
        }

        private double ComputeScoreOfMailByOccurency(string mail, int occurency)
        {
            return OccurencyToScore(occurency);
        }

        private int GetMailPenalty(string mail)
        {
            int penalty = 0;
            if (mail.Contains("faraon.cz") || mail.Contains("realspectrum.cz")) penalty += 100;
            if (mail.Contains("realit")) penalty += 20;
            if (mail.Contains("real")) penalty += 20;
            return penalty;
        }
    }
}