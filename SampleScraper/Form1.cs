using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

//This is required to call 'Extension' methods like 'QuerySelectorAll'
using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;

using System.Net;
using System.IO;


namespace SampleScraper
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ScraperHelper.HttpRequestParam param = new ScraperHelper.HttpRequestParam();
            param.URL = "https://www.olx.com.pk/all-results/";

            String html = ScraperHelper.Helper.GetPagetHTML(param);

            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();

            doc.LoadHtml(html);

            var trs = doc.DocumentNode.QuerySelectorAll("table#offers_table tr").ToList();
            int counter = 0;
            foreach (var tr in trs)
            {

                if (counter < 2)
                {
                    counter++; continue;
                }

                HtmlAgilityPack.HtmlDocument doc1 = new HtmlAgilityPack.HtmlDocument();
                doc1.LoadHtml(tr.InnerHtml);
                var spn = doc1.DocumentNode.QuerySelectorAll("h3 a.detailsLink span").FirstOrDefault();
                if (spn != null)
                {
                    var title = ConvertToText(spn);
                }

                var ptag = doc1.DocumentNode.QuerySelectorAll("p.price").FirstOrDefault();
                if (ptag != null)
                {
                    var price = ConvertToText(ptag);
                }
            }


        }

        public String ConvertToText(HtmlAgilityPack.HtmlNode node)
        {
            String result = "";
            if (node != null & !String.IsNullOrEmpty(node.InnerText))
            {
                result = node.InnerText.Replace("\n", "").Replace("\r", "").Replace("&nbsp;", "").Trim();
            }
            return result;
        }
      
        private void button3_Click(object sender, EventArgs e)
        {
            ScraperHelper.HttpRequestParam param = new ScraperHelper.HttpRequestParam();
            param.URL = "http://pu.edu.pk/home/results_show/2134";
            param.HttpMethodType = ScraperHelper.HttpMethodType.POST;
            param.DataToPost = "result_code=babsc-a2014&result_title=B.A.%2F+B.Sc.+Annual+Examination+2014&roll_no=71204&submitcontact=Submit";



            String html = ScraperHelper.Helper.GetPagetHTML(param);

            HtmlAgilityPack.HtmlWeb htmlWeb = new HtmlAgilityPack.HtmlWeb();
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();

            doc.LoadHtml(html);
        }

      

    }
}
