namespace CapturaLibre;

/// <summary>
/// Botón que conserva el mayor tamaño de letra que cabe en su superficie.
/// Resulta especialmente útil al cambiar de idioma, porque una traducción
/// puede ocupar bastante más espacio que el texto original.
/// </summary>
internal sealed class AutoFitButton : Button
{
    private const float MinimumFontSize = 7.5f;
    private bool _fitting;
    private float _preferredFontSize;
    private FontStyle _preferredFontStyle;
    private string _preferredFontFamily = "Segoe UI";
    private Font? _generatedFont;

    public AutoFitButton()
    {
        _preferredFontSize = Font.Size;
        _preferredFontStyle = Font.Style;
        _preferredFontFamily = Font.FontFamily.Name;
    }

    protected override void OnFontChanged(EventArgs e)
    {
        if (!_fitting)
        {
            _preferredFontSize = Font.Size;
            _preferredFontStyle = Font.Style;
            _preferredFontFamily = Font.FontFamily.Name;
        }
        base.OnFontChanged(e);
        FitTextToButton();
    }

    protected override void OnTextChanged(EventArgs e)
    {
        base.OnTextChanged(e);
        FitTextToButton();
    }

    protected override void OnResize(EventArgs e)
    {
        base.OnResize(e);
        FitTextToButton();
    }

    private void FitTextToButton()
    {
        if (_fitting || IsDisposed || string.IsNullOrWhiteSpace(Text) || ClientSize.Width < 24 || ClientSize.Height < 18)
            return;

        int availableWidth = Math.Max(1, ClientSize.Width - Padding.Horizontal - 18);
        int availableHeight = Math.Max(1, ClientSize.Height - Padding.Vertical - 10);
        float selectedSize = _preferredFontSize;

        for (float candidate = _preferredFontSize; candidate >= MinimumFontSize; candidate -= 0.5f)
        {
            using var testFont = new Font(_preferredFontFamily, candidate, _preferredFontStyle, GraphicsUnit.Point);
            Size measured = TextRenderer.MeasureText(Text, testFont,
                new Size(availableWidth, int.MaxValue),
                TextFormatFlags.WordBreak | TextFormatFlags.HorizontalCenter | TextFormatFlags.NoPadding);
            selectedSize = candidate;
            if (measured.Width <= availableWidth && measured.Height <= availableHeight)
                break;
        }

        if (Math.Abs(Font.Size - selectedSize) < 0.1f &&
            Font.Style == _preferredFontStyle && Font.FontFamily.Name == _preferredFontFamily)
            return;

        _fitting = true;
        Font? previousGenerated = _generatedFont;
        _generatedFont = new Font(_preferredFontFamily, selectedSize, _preferredFontStyle, GraphicsUnit.Point);
        Font = _generatedFont;
        _fitting = false;
        previousGenerated?.Dispose();
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _generatedFont?.Dispose();
            _generatedFont = null;
        }
        base.Dispose(disposing);
    }
}
