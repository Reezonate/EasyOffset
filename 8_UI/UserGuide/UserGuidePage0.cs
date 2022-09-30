using BeatSaberMarkupLanguage.Attributes;
using JetBrains.Annotations;

namespace EasyOffset;

internal class UserGuidePage0 : UserGuidePage {
    #region UserGuidePage

    public override string Title => "Intro";
    
    #endregion

    #region Text

    [UIValue("text"), UsedImplicitly]
    private string _text = "Welcome to the Easy Offset! " +
                           "\r\n\r\n" +
                           "In this step-by-step video guide you will learn how to make your own controller settings";

    #endregion
}