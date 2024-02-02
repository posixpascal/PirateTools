using PdfSharpCore.Pdf;
using PdfSharpCore.Pdf.AcroForms;
using System.Globalization;
using System.Threading.Tasks;

namespace PirateTools.Models.PDF;

public abstract class PdfFormBuilder {
    public abstract Task BuildPdfFormAsync(PdfAcroForm form, TravelExpenseReport report,
        CultureInfo culture, int attachmentCount);

    protected static void SetField(PdfAcroForm form, string field, string value) {
        form.Fields[field].Value = new PdfString(value, PdfStringEncoding.Unicode);
    }

    protected static void SetBool(PdfAcroForm form, string field, bool value) {
        form.Fields[field].Value = new PdfBoolean(value);
    }
}