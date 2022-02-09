namespace EasyOffset; 

public readonly struct ButtonAliasPair {
    public readonly ControllerButton ControllerButton;
    public readonly string AliasName;
    
    public ButtonAliasPair(ControllerButton controllerButton, string aliasName) {
        ControllerButton = controllerButton;
        AliasName = aliasName;
    }
}