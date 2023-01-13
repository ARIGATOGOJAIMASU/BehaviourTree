using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;
#if UNITY_EDITOR
public class SpiltView : TwoPaneSplitView
{
    public new class UxmlFactory : UxmlFactory<SpiltView, TwoPaneSplitView.UxmlTraits> { }
}
#endif
