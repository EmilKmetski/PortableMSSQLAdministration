using PortableMSSQLAdministration.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;


namespace PortableMSSQLAdministration
{

        
    public class SQLFormmatterGroupBoxDraw
    {   
        //changeit to work with Wpf 
        public bool DrawGroupBoxElements(string confFilePath)
        {
            DataTable formatConfiguration = ReadExcelFile.GetExcellToDtbl(confFilePath);
            Point previousPoint = new Point(6, 0);
            Point firstRow = new Point(6, 0);
            int maxY = 407;
            int controlHeight = 25;
            int currentCol = 1;
            int columnMaxLen = 0;
            foreach (DataRow dr in formatConfiguration.Rows)
            {
                if (dr[3].ToString().ToUpper() == "TRUE OR FALSE")
                {
                    CheckBox myCb = new CheckBox();
                    myCb.Name = dr[1].ToString();
                    myCb.Content = dr[1].ToString();
                    myCb.BackColor = Color.Black;
                    myCb.ForeColor = Color.MediumTurquoise;

                   

                    if (((previousPoint.Y + controlHeight - 5) < maxY) && currentCol == 1)
                    {
                        previousPoint.X = previousPoint.X;
                        previousPoint.Y = previousPoint.Y + controlHeight;
                        myCb.Location = previousPoint;
                    }
                    else
                    {
                        previousPoint.X = firstRow.X + currentCol * 10 + 260;
                        previousPoint.Y = firstRow.Y + controlHeight;
                        myCb.Location = previousPoint;
                    }
                    
                        if (int.Parse(dr[0].ToString()) == 1)
                        {
                            myCb.IsChecked = true;
                        }
                        else
                        {
                            myCb.IsChecked = false;
                        }
                    
                    this.gbFormatSettings.Controls.Add(myCb);
                }
                else if (dr[3].ToString().ToUpper() == "SqlVersion".ToUpper() || dr[3].ToString().ToUpper() == "KeywordCasing".ToUpper())
                {
                    Label myComboLable = new Label();
                    myComboLable.Text = dr[1].ToString();
                    ComboBox myCombo = new ComboBox();
                    myCombo.Name = dr[1].ToString();
                    myCombo.AutoSize = true;
                    myCombo.DropDownStyle = ComboBoxStyle.DropDownList;
                    myCombo.BackColor = Color.Black;
                    myCombo.ForeColor = Color.MediumTurquoise;

                    if (dr[3].ToString().ToUpper() == "SqlVersion".ToUpper())
                    {
                        myCombo.Items.Add("SQL 2005");
                        myCombo.Items.Add("SQL 2000");
                        myCombo.Items.Add("SQL 2008");
                        myCombo.Items.Add("SQL 2012");
                        myCombo.Items.Add("SQL 2014");
                        myCombo.Items.Add("SQL 2016");
                    }
                    else
                    {
                        myCombo.Items.Add("Lowercase");
                        myCombo.Items.Add("Uppercase");
                        myCombo.Items.Add("PascalCase");
                    }
                    if (drawFromFile)
                    {
                        myCombo.SelectedIndex = int.Parse(dr[0].ToString());
                    }

                    if (((previousPoint.Y + controlHeight - 5) < maxY) && currentCol == 1)
                    {
                        previousPoint.X = previousPoint.X;
                        previousPoint.Y = previousPoint.Y + controlHeight;
                        myComboLable.Location = previousPoint;
                        myCombo.Location = new Point(previousPoint.X, previousPoint.Y + 25);
                        previousPoint.Y = previousPoint.Y + 25;
                    }
                    else
                    {
                        previousPoint.X = firstRow.X + currentCol * 10 + 260;
                        previousPoint.Y = firstRow.Y + controlHeight;
                        myComboLable.Location = previousPoint;
                        myCombo.Location = new Point(previousPoint.X, previousPoint.Y + 25);
                        previousPoint.Y = firstRow.Y + 25;
                    }
                    this.gbFormatSettings.Controls.Add(myComboLable);
                    this.gbFormatSettings.Controls.Add(myCombo);
                }
                else if (dr[3].ToString().ToUpper() == "Indent".ToUpper())
                {
                    Label myTextLable = new Label();
                    myTextLable.Text = dr[1].ToString();
                    TextBox mytext = new TextBox();
                    mytext.Name = dr[1].ToString();
                    mytext.BackColor = Color.Black;
                    mytext.ForeColor = Color.MediumTurquoise;

                    if (drawFromFile)
                    {
                        mytext.Text = dr[0].ToString();
                    }
                    mytext.AutoSize = true;
                    if (((previousPoint.Y + controlHeight - 5) < maxY) && currentCol == 1)
                    {
                        previousPoint.X = previousPoint.X;
                        previousPoint.Y = previousPoint.Y + controlHeight;
                        myTextLable.Location = previousPoint;
                        mytext.Location = new Point(previousPoint.X, previousPoint.Y + 25);
                        previousPoint.Y = previousPoint.Y + 25;
                    }
                    else
                    {
                        previousPoint.X = firstRow.X + currentCol * 10 + 260;
                        previousPoint.Y = firstRow.Y + controlHeight;
                        myTextLable.Location = previousPoint;
                        mytext.Location = new Point(previousPoint.X, previousPoint.Y + 25);
                        previousPoint.Y = firstRow.Y + 25;
                    }
                    this.gbFormatSettings.Controls.Add(myTextLable);
                    this.gbFormatSettings.Controls.Add(mytext);
                }
            }
            System.Diagnostics.Debug.WriteLine(columnMaxLen);
            return true;
        }
    }
}
