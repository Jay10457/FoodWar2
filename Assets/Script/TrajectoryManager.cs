using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryManager : MonoBehaviour
{

    public enum lineMode
    {
        DrawRayEditorOnly = 1,
        LineRendererBoth = 2
    }
    public enum predictionMode
    {
        Prediction2D = 2,
        Prediction3D = 3
    };
    #region LineSetting
    [Header("Line Settings")]
    [Tooltip("The type of line to draw for debug stuff. DrawRay: uses built in Debug.DrawRay only visible in editor." +
        " LineRenderer: uses a line renderer on a separate created GameObject to draw the line, is visble in editor and play mode")]
    public lineMode debugLineMode = lineMode.LineRendererBoth;
    [Tooltip("Draw a debug line on object start? (Requires a rigidbody or rigidbody2D)")]
    public bool drawDebugOnStart = false;
    [Tooltip("Draw a debug line on object update? (Requires a rigidbody or rigidbody2D)")]
    public bool drawDebugOnUpdate = false;
    [Tooltip("Draw a debug line when predicting the trajectory")]
    public bool drawDebugOnPrediction = false;
    [Tooltip("Duration the prediction line lasts for. When predicting every frame its a good idea to update this value to Time.unscaledDeltaTime every frame."
        + "(This is done automatically if you use the drawDebugOnUpdate option)")]
    public float debugLineDuration = 4f;
    [Tooltip("Number of frames that pass before the line is refreshed. Increasing this number could significantly improve performance with a large amount of lines being predicted at once."
        + "(Only used if drawDebugOnUpdate is enabled.)")]
    [Range(1, 10)]
    public int debugLineUpdateRate = 1;
    [Tooltip("If using the linerenderer, will reuse the gameobject and line renderer components instead of destroying and recreating them every time. " +
        "This option improves performance for multiple succesive predictions DON'T use this for one-off predictions, " +
        "as it will not take line duration into account and the line will stick around forever until the component is destroyed. " +
        "NOTE: this option is automatically used by drawDebugOnUpdate and does not need to be enabled here for that to work.")]
    public bool reuseLine = false;
    [Tooltip("The name of the layer the line is drawn on. Only checked once on start")]
    public string lineSortingLayerName;
    private int lineSortingLayer = 0;

    [Tooltip("The order in the sorting layer the line is drawn.")]
    public int lineSortingOrder = 0;

    [Header("Line Appearance")]
    [Tooltip("Thickness of the debug line when using the line renderer mode.")]
    public float lineWidth = 0.05f;
    [Tooltip("Start color of the debug line")]
    public Color lineStartColor = Color.white;
    [Tooltip("End color of the debug line")]
    public Color lineEndColor = Color.white;
    [Tooltip("If provided, this shader will be used on the LineRenderer. (Recommended is the particles section of shaders)")]
    public Shader lineShader;
    [Tooltip("If provided, this texture will be added to the material of the LineRenderer. (A couple textures come packaged with the script)")]
    public Texture lineTexture;
    [Tooltip("Value to scale the tiling of the line texture by.")]
    public float textureTilingMult = 1f;
    #endregion

    [Header("Prediction Settings")]
    [Range(0.7f, 0.9999f)]
    [Tooltip("The accuracy of the prediction. This controls the distance between steps in calculation.")]
    public float accuracy = 0.98f;
    [Tooltip("Limit on how many steps the prediction can take before stopping.")]
    public int iterationLimit = 150;
    [Tooltip("Whether the prediction should be a 2D or 3D line.")]
    public predictionMode predictionType = predictionMode.Prediction3D;
    [Tooltip("The layer mask to use for raycasting when calculating the prediction. This setting only matters when checkForCollision is on.")]
    public LayerMask raycastMask = -1;
    [Tooltip("Stop the prediction where the line hits an object? This check works by using raycasts, so you can use the mask and putting objects on different layers to make them not be checked for collision.")]
    public bool checkForCollision = true;

}
