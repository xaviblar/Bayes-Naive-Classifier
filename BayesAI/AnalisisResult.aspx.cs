using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;

namespace BayesAI
{
    public partial class About : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            myChart.Titles.Add("Grafico");
            ChartArea chartArea1 = new ChartArea();
            chartArea1.Name = "PieChart";
            myChart.ChartAreas.Add(chartArea1);
            Series series1 = new Series();
            series1.Name = "Series1";
            series1.ChartType = SeriesChartType.Pie;
            myChart.Series.Add(series1);
            Legend legend1 = new Legend();
            legend1.Name = "PieChart";
            legend1.Docking = Docking.Bottom;
            legend1.LegendStyle = LegendStyle.Row;
            legend1.Alignment = System.Drawing.StringAlignment.Center;
            legend1.BackColor = System.Drawing.Color.Transparent;
            legend1.BorderColor = System.Drawing.Color.Black;
            legend1.BorderWidth = 1;
            myChart.Legends.Add(legend1);

            Random rnd = new Random();

            foreach (Series ser in myChart.Series)
            {
                for (int i = 0; i < 5; i++)
                {
                    ser.Points.AddXY("punto" + i.ToString(), rnd.Next(5, 40));

                }
            }
        }
    }
}