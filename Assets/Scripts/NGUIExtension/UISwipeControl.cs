using System;
using UnityEngine;

/// <summary>
/// Handles swipe control 
/// </summary>

[AddComponentMenu("NGUI/PunishPanda/Panda Swipe Control")]
public class UISwipeControl : MonoBehaviour
{
    /// <summary>
    /// The strength of the spring.
    /// </summary>

    public float springStrength = 8f;

    /// <summary>
    /// Callback to be triggered when the centering operation completes.
    /// </summary>

    public SpringPanel.OnFinished onFinished;

    UIDraggablePanel mDrag;
    GameObject mCenteredObject;

    public UILabel Label;

    private string XofYString;

    /// <summary>
    /// Game object that the draggable panel is currently centered on.
    /// </summary>

    public GameObject centeredObject { get { return mCenteredObject; } }

    void OnEnable()
    {
        XofYString = " " + Localization.instance.Get("Of") + " ";
        if (transform.childCount > 0)
        {
            Recenter(transform.GetChild(0));
            //Label.text = 1 + XofYString + transform.childCount;
        }
        else
        {
            Recenter();
        }
    }

    void Start()
    {
        
    }

    void OnDragFinished()
    {
        if (enabled)
        {
            touchEndPos = UICamera.currentTouch.pos;
            Recenter();
        }
    }

    private Vector2 touchStartPos;
    private Vector2 touchEndPos;
    private Transform startClosetsChild;
    private int startClosetsChildIndex;

    public bool HasReachedRightEnd
    {
        get
        {
            return startClosetsChildIndex == transform.childCount - 1;
        }
    }

    public float SwipeMinimum;



    /// <summary>
    /// Recenter the draggable list on the center-most child.
    /// </summary>

    public void Recenter(Transform target = null, bool buttonNext = false)
    {
        if (mDrag == null)
        {
            mDrag = NGUITools.FindInParents<UIDraggablePanel>(gameObject);

            if (mDrag == null)
            {
                Debug.LogWarning(GetType() + " requires " + typeof(UIDraggablePanel) + " on a parent object in order to work", this);
                enabled = false;
                return;
            }
            else
            {
                mDrag.onDragFinished = OnDragFinished;
                mDrag.onDragStarted = OnDragStarted;

                if (mDrag.horizontalScrollBar != null)
                    mDrag.horizontalScrollBar.onDragFinished = OnDragFinished;

                if (mDrag.verticalScrollBar != null)
                    mDrag.verticalScrollBar.onDragFinished = OnDragFinished;
            }
        }
        if (mDrag.panel == null) 
            return;


        // Calculate the panel's center in world coordinates
        Vector4 clip = mDrag.panel.clipRange;
        Transform dt = mDrag.panel.cachedTransform;
        Vector3 center = dt.localPosition;
        center.x += clip.x;
        center.y += clip.y;
        center = dt.parent.TransformPoint(center);

        // Offset this value by the momentum
        Vector3 offsetCenter = center - mDrag.currentMomentum * (mDrag.momentumAmount * 0.1f);
        mDrag.currentMomentum = Vector3.zero;

        float min = float.MaxValue;

        if (!buttonNext)
        {
            if(target == null)
            { 
            //Debug.Log(touchEndPos + " " + touchStartPos);
                if (Vector2.Distance(touchEndPos, touchStartPos) >= SwipeMinimum)
                {

                    if (touchStartPos.x < touchEndPos.x) //Move left
                    {
                        if (startClosetsChildIndex == 0) //We are at the left end of the panel, target ourself
                        {
                            target = startClosetsChild;
                        }
                        else //Else Move one to the left
                        {
                            startClosetsChildIndex -= 1;
                            target = transform.GetChild(startClosetsChildIndex);
                        }
                    }
                    else if (touchStartPos.x >= touchEndPos.x) //Move left
                    {
                        if (startClosetsChildIndex == transform.childCount - 1)
                            //We are at the right end of the panel, target ourself
                        {
                            target = startClosetsChild;
                        }
                        else //Else Move one to the left
                        {
                            startClosetsChildIndex += 1;
                            target = transform.GetChild(startClosetsChildIndex);

                        }
                    }
                }
            }
        }
        else
        {
            if (startClosetsChildIndex == transform.childCount - 1) //We are at the right end of the panel, target ourself
            {
                target = startClosetsChild;
            }
            else //Else Move one to the left
            {
                startClosetsChildIndex += 1;
                target = transform.GetChild(startClosetsChildIndex);

            }
        }

        if (target != null)
        {
            int index;
            // Determine the closest child
            target = ClosetsChild(target, offsetCenter, min, out index);

            if (Label != null)
            {
                Label.text = (startClosetsChildIndex + 1) + XofYString + transform.childCount;
            }
        }

        if (target == null)
        {
            if (mCenteredObject != null)
                target = mCenteredObject.transform;
            else
                target = gameObject.transform;
        }

        if (target != null)
        {
            mCenteredObject = target.gameObject;

            // Figure out the difference between the chosen child and the panel's center in local coordinates
            Vector3 cp = dt.InverseTransformPoint(target.position);
            Vector3 cc = dt.InverseTransformPoint(center);
            Vector3 offset = cp - cc;

            // Offset shouldn't occur if blocked by a zeroed-out scale
            if (mDrag.scale.x == 0f) offset.x = 0f;
            if (mDrag.scale.y == 0f) offset.y = 0f;
            if (mDrag.scale.z == 0f) offset.z = 0f;

            // Spring the panel to this calculated position
            SpringPanel.Begin(mDrag.gameObject, dt.localPosition - offset, springStrength).onFinished = onFinished;
        }
        
    }

    private static Transform ClosetsChild(Transform trans, Vector3 offsetCenter, float min, out int index)
    {
        Transform target = null;
        index = -1;
        for (int i = 0, imax = trans.childCount; i < imax; ++i)
        {
            Transform t = trans.GetChild(i);
            float sqrDist = Vector3.SqrMagnitude(t.position - offsetCenter);

            if (sqrDist < min)
            {
                index = i;
                min = sqrDist;
                target = t;
            }
        }
        
        return target;
    }

    public void GoToNext()
    {
        Recenter(null, true);
    }

    private void OnDragStarted()
    {
        Vector4 clip = mDrag.panel.clipRange;
        Transform dt = mDrag.panel.cachedTransform;
        Vector3 center = dt.localPosition;
        center.x += clip.x;
        center.y += clip.y;
        center = dt.parent.TransformPoint(center);
        Vector3 offsetCenter = center - mDrag.currentMomentum * (mDrag.momentumAmount * 0.1f);
        startClosetsChild = ClosetsChild(transform, offsetCenter, float.MaxValue, out startClosetsChildIndex);
        touchStartPos = UICamera.currentTouch.pos;
    }

    public void RedoLabel()
    {
        Label.text = (startClosetsChildIndex + 1) + XofYString + transform.childCount;
    }
}