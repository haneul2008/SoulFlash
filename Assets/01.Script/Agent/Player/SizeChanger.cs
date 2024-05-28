using UnityEngine;
public class SizeChanger
{
    private Vector2 _saveSize;
    public Vector2 ChangeSize(Vector2 saveSize, Vector2 nextSize)
    {
        _saveSize = saveSize;
        return nextSize;
    }
    public Vector2 GetSaveSize()
    {
        return _saveSize;
    }
}
