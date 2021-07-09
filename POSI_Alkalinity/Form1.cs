using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POSI_Alkalinity
{
    public partial class Form1 : Form
    {
        public double Ca = 305;//78.0;//补充水钙硬度（按碳酸钙计）124.8
        public double Mg = 153;//19.0;//补充水镁硬度（按碳酸钙计）37.32
        public double Na = 100.0;//补充水钠离子100
        public double Cl = 180;//100.0;//补充水氯离子180
        public double Clmax = 1000.0;//循环水氯离子限值1000
        public double Cond = 1364;//1000.0;//补充水电导率1364
        public double Alkalinity_Max = 400;//680.0;//循环水碱度控制限值400
        public double Alkalinity_Input = 549;//198.0;//补充水碱度
        public double Alkalinity = 0.0;//POSI计算出的补充水碱度（当量）
        public double SO4 = 50.0;//补充水硫酸根
        public double SiO2 = 50.0;//补充水硅酸

        public const double C_Ca = 2.45;//碳酸钙与钙离子转换常数
        public const double C_Mg = 4.11;//碳酸钙与镁离子转换常数
        public const double Ca_Alkalinity_Max = 1100.0;//水钙硬+全碱度限制
        public const double Cl_SO4_Max = 2500.0;//氯离子+硫酸根限制
        public const double Mg_SiO2_Max = 50000.0;//镁离子乘以硅酸限制

        public const double Delta_Coc = 0.01;//

        public Form1()
        {
            InitializeComponent();//窗口初始化
            double COC_Cl = Math.Round((Clmax / Cl), 2);//计算按氯离子限制最大浓缩倍数

            double Ca_Alkalinity = (Ca + Alkalinity) * COC_Cl;//计算循环水钙硬+全碱度
            double Cl_SO4 = (Cl + SO4) * COC_Cl;//计算循环水氯离子+硫酸根
            double Mg_SiO2 = (Mg * SiO2) * COC_Cl;//计算镁离子+硅酸

            do//按POSI计算循环水碱度限值
            {
                Alkalinity = Math.Round((COC_Cl * Math.Pow(10, (1 / (Math.Log10(COC_Cl / 1.5) * Math.Log10(Ca * Mg / (Cl + Na))) + 1))), 2);
                COC_Cl = COC_Cl - Delta_Coc;
            } while (Alkalinity_Max > Alkalinity);//比较碱度限值和碱度控制上限
            COC_Cl += Delta_Coc;//调回一格步长

            //do
            //{
            //    Alkalinity = Math.Round((COC_Cl * Math.Pow(10, (1 / (Math.Log10(COC_Cl / 1.5) * Math.Log10(Ca * Mg / (Cl + Na))) + 1))), 2);
            //    Ca_Alkalinity = (Ca + Alkalinity) * COC_Cl;
            //    //Cl_SO4 = (Cl + SO4) * COC_Cl;
            //    //Mg_SiO2 = (Mg * SiO2) * COC_Cl;
            //    COC_Cl = COC_Cl - Delta_Coc;
            //} while (Ca_Alkalinity > Ca_Alkalinity_Max);
            //COC_Cl += Delta_Coc;

            //do
            //{
            //    //Ca_Alkalinity = (Ca + Alkalinity) * COC_Cl;
            //    Cl_SO4 = (Cl + SO4) * COC_Cl;
            //    //Mg_SiO2 = (Mg * SiO2) * COC_Cl;
            //    COC_Cl = COC_Cl - Delta_Coc;
            //} while (Cl_SO4 > Cl_SO4_Max);
            //COC_Cl += Delta_Coc;

            //do
            //{
            //    //Ca_Alkalinity = (Ca + Alkalinity) * COC_Cl;
            //    //Cl_SO4 = (Cl + SO4) * COC_Cl;
            //    Mg_SiO2 = (Mg * SiO2) * COC_Cl;
            //    COC_Cl = COC_Cl - Delta_Coc;
            //} while (Mg_SiO2 > Mg_SiO2_Max);
            //COC_Cl += Delta_Coc;

            COC_Cl = Math.Round(COC_Cl, 2);//将逼近计算后COC保留两位小数

            string result = "最大浓缩倍数：" + COC_Cl.ToString();//生成需要展示的计算结果字符串
            label1.Text = result;//将字符串赋值入控件
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();//关闭窗口
        }
    }
}
