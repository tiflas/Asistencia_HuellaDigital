using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppMonitoreo.Controller
{
    class ExportarExcel
    {
        public void ExportarAExcel(DataGridView Grd)
        {
            try
            {
                SaveFileDialog Fichero = new SaveFileDialog();
                Fichero.Filter = "Excel (*.xlsx)|*.xlsx"; // Cambiamos la extensión a xlsx
                Fichero.FileName = "ArchivoExportado";
                if (Fichero.ShowDialog() == DialogResult.OK)
                {
                    Microsoft.Office.Interop.Excel.Application aplicacion;
                    Microsoft.Office.Interop.Excel.Workbook libros_trabajo;
                    Microsoft.Office.Interop.Excel.Worksheet hoja_trabajo;

                    aplicacion = new Microsoft.Office.Interop.Excel.Application();
                    libros_trabajo = aplicacion.Workbooks.Add();
                    hoja_trabajo = (Microsoft.Office.Interop.Excel.Worksheet)libros_trabajo.Worksheets.get_Item(1);

                    // Exportar encabezados de columnas
                    for (int col = 0; col < Grd.Columns.Count; col++)
                    {
                        hoja_trabajo.Cells[1, col + 1] = Grd.Columns[col].HeaderText;
                    }

                    // Exportar datos del DataGridView
                    for (int i = 0; i < Grd.Rows.Count; i++)
                    {
                        for (int j = 0; j < Grd.Columns.Count; j++)
                        {
                            if (Grd.Rows[i].Cells[j].Value != null)
                            {
                                hoja_trabajo.Cells[i + 2, j + 1] = Grd.Rows[i].Cells[j].Value.ToString();
                            }
                        }
                    }

                    // Guardar como archivo xlsx
                    libros_trabajo.SaveAs(Fichero.FileName, Microsoft.Office.Interop.Excel.XlFileFormat.xlOpenXMLWorkbook);
                    libros_trabajo.Close(true);
                    aplicacion.Quit();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al Exportar la Información debido a: " + ex.ToString());
            }
        } 
    }
}
