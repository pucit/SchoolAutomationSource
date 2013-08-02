using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using School_Management_System;
using System.IO;
using System.Data;
using System.Drawing;
using System.Reflection;
using Novacode;
using School_Management_System.Models;

namespace School_Management_System.Controllers
{
    class Programm
    {
        /* 
         * Place holder for the executing assembly, 
         * we need this to access embedded resources.
         */
        static Assembly g_assembly;

        // Place holder for a document.
        static DocX g_document;

        static void Main(string[] args)
        {
            // Store a global reference to the executing assembly.
            g_assembly = Assembly.GetExecutingAssembly();

            // Try to load the template 'InvoiceTemplate.docx'.
            try
            {
                // Store a global reference to the loaded document.
                g_document = DocX.Load(@"Content/letterheadpad_template.docx");

                /*
                 * The template 'InvoiceTemplate.docx' exists, 
                 * so lets use it to create an invoice for a factitious company
                 * called "The Happy Builder" and store a global reference it.
                 */
                g_document = CreateInvoiceFromTemplate(DocX.Load(@"Content/letterheadpad_template.docx"));

                // Save all changes made to this template as Invoice_The_Happy_Builder.docx (We don't want to replace InvoiceTemplate.docx).
                g_document.SaveAs("Report_print.docx");
            }

            // The template 'InvoiceTemplate.docx' does not exist, so create it.
            catch (FileNotFoundException)
            {
                // Create a store a global reference to the template 'InvoiceTemplate.docx'.
                Console.WriteLine("FILE NOT FOUNDDDDDDDDDDDDDDDDDDDDDDDD");

            }
        }

        // Create an invoice for a factitious company called "The Happy Builder".
        private static DocX CreateInvoiceFromTemplate(DocX template)
        {            
            Table t = template.Tables[0];
            Table invoice_table = CreateAndInsertInvoiceTableAfter(t, ref template);
            t.Remove();

            // Return the template now that it has been modified to hold all of our custom data.
            return template;
        }

        
        private static Table CreateAndInsertInvoiceTableAfter(Table t, ref DocX document)
        {
            // Grab data from somewhere (Most likely a database)
            schooldbEntities DAO = new schooldbEntities();
            List<teacher> tlist = (from tt in DAO.teachers select tt).ToList<teacher>();            
            
            /* 
             * The trick to replacing one Table with another,
             * is to insert the new Table after the old one, 
             * and then remove the old one.
             */
            Table invoice_table = t.InsertTableAfterSelf(tlist.Count + 1, 4);
            invoice_table.Design = TableDesign.LightShadingAccent1;

            #region Table title
            Formatting table_title = new Formatting();
            table_title.Bold = true;

            invoice_table.Rows[0].Cells[0].Paragraph.InsertText("Serial No.", false, table_title);
            invoice_table.Rows[0].Cells[0].Paragraph.Alignment = Alignment.center;
            invoice_table.Rows[0].Cells[1].Paragraph.InsertText("Employee Name", false, table_title);
            invoice_table.Rows[0].Cells[1].Paragraph.Alignment = Alignment.center;
            invoice_table.Rows[0].Cells[2].Paragraph.InsertText("Account No.", false, table_title);
            invoice_table.Rows[0].Cells[2].Paragraph.Alignment = Alignment.center;
            invoice_table.Rows[0].Cells[3].Paragraph.InsertText("Salary", false, table_title);
            invoice_table.Rows[0].Cells[3].Paragraph.Alignment = Alignment.center;
            #endregion

            // Loop through the rows in the Table and insert data from the data source.
            for (int row = 1; row < tlist.Count; row++)
            {
                
                    Paragraph cell_paragraph = invoice_table.Rows[row].Cells[0].Paragraph;
                    cell_paragraph.InsertText(row.ToString(), false);

                    cell_paragraph = invoice_table.Rows[row].Cells[1].Paragraph;
                    cell_paragraph.InsertText(tlist[row - 1].TeacherName.ToString(), false);

                    cell_paragraph = invoice_table.Rows[row].Cells[2].Paragraph;
                    cell_paragraph.InsertText(tlist[row - 1].Account_Number.ToString(), false);

                    cell_paragraph = invoice_table.Rows[row].Cells[3].Paragraph;
                    cell_paragraph.InsertText(tlist[row - 1].BasicSalary.ToString(), false);
                
            }

            

            // Let the tables coloumns expand to fit its contents.
            invoice_table.AutoFit = AutoFit.Contents;

            // Center the Table
            invoice_table.Alignment = Alignment.center;

            // Return the invloce table now that it has been created.
            return invoice_table;
        }

        
    }
}
