namespace Assets.Scripts
{
    using Assets.Scripts.Flight.GameView.Cameras;
    using ModApi.Ui.Inspector;
    using Assets.Packages.DevConsole;
    using Assets.Scripts.Craft.Parts.Modifiers.Input;
    using ModApi.Craft.Parts.Input;
    using UnityEngine;
    using ModApi.Settings.Core.Events;
    using ModApi.Settings.Core;

    public class Mod : ModApi.Mods.GameMod
    {
        private Mod() : base() { }
        public static Mod Instance { get; } = GetModInstance<Mod>();
        private float fov;
        private static IInputControllerInput fovInput;
        SliderModel fovSlider;
        private StringSetting fovInputSetting => ModSettings.Instance.fovInput;

        private float getFov()
        {
            if (fovInputSetting.Value == "FovSlider" || fovInput == null)
                return fovSlider.Value;
            else
            {
                RefreshInput();
                return fovInput.Value;
            }
        }

        protected override void OnModInitialized()
        {
            base.OnModInitialized();
            Game.Instance.UserInterface.AddBuildInspectorPanelAction(ModApi.Ui.Inspector.InspectorIds.FlightView, OnBuildFlightViewInspectorPanel);

            DevConsoleApi.RegisterCommand("SetFovInput", delegate (string input)
            {
                if (input != "FovSlider") fovInput = InputControllerInput.Create(input);
                if (fovInput != null)
                {
                    fovInputSetting.Value = input;
                    ModSettings.Instance.CommitChanges();
                    Debug.Log("Fov input was set to " + input);
                }
                else Debug.Log("Fov input is not valid");
            });

            ModSettings.Instance.Changed += OnSettingsChanged;

            if (fovInputSetting.Value != "FovSlider") fovInput = InputControllerInput.Create(fovInputSetting.Value);
        }

        private void OnSettingsChanged(object sender, SettingsChangedEventArgs<ModSettings> e)
        {
            if (fovInputSetting.Value != "FovSlider") fovInput = InputControllerInput.Create(fovInputSetting.Value);
        }

        private void OnBuildFlightViewInspectorPanel(BuildInspectorPanelRequest request)
        {
            fov = Game.Instance.Settings.Game.General.FieldOfView;

            var g = new GroupModel("Camera Tools");
            request.Model.AddGroup(g);
            g.Collapsed = true;

            fovSlider = new SliderModel("Fov", () => fov, delegate (float x)
            {
                if (fovInputSetting.Value == "FovSlider")
                {
                    fov = x;
                    CameraManagerScript.Instance.FieldOfView = fov;
                }

            }, 0.5f, 150);
            g.Add(fovSlider);
            fovSlider.ValueFormatter = ((float x) => $"{(fovSlider.Value):n2}");
        }

        public void Update()
        {
            if (Game.InFlightScene)
                if (getFov() != fov)
                {
                    fov = getFov();
                    CameraManagerScript.Instance.FieldOfView = fov;
                }
        }

        private void RefreshInput()
        {
            if (fovInput is InputControllerInput)
                ((InputControllerInput)fovInput).RefreshInput(Game.Instance.FlightScene.CraftNode.CraftScript.RootPart);
            if (fovInput is InputControllerExpression)
                ((InputControllerExpression)fovInput).RefreshInput(Game.Instance.FlightScene.CraftNode.CraftScript.RootPart);
            if (fovInput is InputControllerInputPartModifierWrapper)
                ((InputControllerInputPartModifierWrapper)fovInput).RefreshInput(Game.Instance.FlightScene.CraftNode.CraftScript.RootPart);
        }
    }
}