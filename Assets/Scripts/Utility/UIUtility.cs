using UnityEngine.EventSystems;

public static class UIUtility
{
    public static bool IsMouseOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
}