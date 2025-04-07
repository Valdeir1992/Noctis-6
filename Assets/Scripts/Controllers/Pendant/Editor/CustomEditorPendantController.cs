using UnityEditor;
using UnityEngine.UIElements;

[CustomEditor(typeof(PendantController))]
public class CustomEditorPendantController : Editor {
    public VisualTreeAsset VisualTreeAssetBase;

    public override VisualElement CreateInspectorGUI()
    {
        var root = new VisualElement();
        VisualTreeAssetBase.CloneTree(root);
        int index = 0;
        var buttons = root.Q<Foldout>("Nimeryus_Foldout").contentContainer.Query<Button>().ToList();
        foreach (var nimeryusButtonTransition in buttons)
        {
            var selected = index;
            nimeryusButtonTransition.clicked += () => ((PendantController)target).NimeryusCrystalsTransition(selected);
            index++;
        }
        var equimonButtonTransition = root.Q<Foldout>("Equimon_Foldout").contentContainer.Q<Button>();
        equimonButtonTransition.clicked += () => ((PendantController)target).EquimonCrystalTransition();
        return root;
    }
}
