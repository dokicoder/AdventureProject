using System;
using UnityEngine;
using TMPro;
using Yarn.Unity;

/**
 * SelectionLabelController is currently written in a way that tracks ownership of the label and warns if multiple instances try to access it at the same time
 * TODO: it could make sense to let Behaviours spawn and control their own labels
*/
[DisallowMultipleComponent]
[RequireComponent(typeof (TextMeshProUGUI))]
public class SelectionLabelController : MonoBehaviour
{
    public Vector3 TopOffset = new Vector3(0, 30, 0);
    
    private GameObject owner;

    private GameObject labelGO;
    private GameObject LabelGO
    {
        get
        {
            if (labelGO == null)
            {
                labelGO = GameObject.FindWithTag("SelectionLabel");
            }
            return labelGO;
        }
    }

    private void Awake() 
    {
        LabelGO.gameObject.SetActive(false);
    }

    public static Vector3 CalculateLabelScreenPosition( SpriteRenderer targetRenderer, Vector3 topOffset )
    {
        // TODO: using localScale here is probably not strictly correct. I don't see a reason for nested scaling though so it should suffice
        float halfBoundsHeight = targetRenderer.sprite.bounds.size.y * targetRenderer.transform.lossyScale.y * 0.5f;
        Vector3 meshTopScreenPosition = Camera.main.WorldToScreenPoint(targetRenderer.transform.position + new Vector3(0, halfBoundsHeight, 0));

        return meshTopScreenPosition + topOffset;
    }

    public static Vector3 CalculateLabelScreenPosition( Renderer targetRenderer, Vector3 topOffset )
    {
        // TODO: using localScale here is probably not strictly correct. I don't see a reason for nested scaling though so it should suffice
        float halfBoundsHeight = targetRenderer.bounds.size.y * targetRenderer.transform.lossyScale.y * 0.5f;
        Vector3 meshTopScreenPosition = Camera.main.WorldToScreenPoint(targetRenderer.transform.position + new Vector3(0, halfBoundsHeight, 0));

        return meshTopScreenPosition + topOffset;
    }

    public void RequestOwnership(GameObject requester) 
    {
        //Debug.LogFormat("SelectionLabelController: ownership was requested by {0}", requester.name);
      
        if(owner != null) {
            Debug.LogWarningFormat("Warning: SelectionLabelController ownership was requested by {0} while still owned by {1}", requester.name, owner.name);
        }

        owner = requester;
    }

    public void SubmitOwnership(GameObject requester) 
    {
        //Debug.LogFormat("SelectionLabelController: ownership was submitted by {0}", requester.name);

        if(owner != null && requester != owner) {
            Debug.LogWarningFormat("Warning: SelectionLabelController ownership was submitted by {0}, but already owned by {1}", requester.name, owner.name);

            return;
        }

        owner = null;
    }

    public void UpdateLabelPosition(GameObject requester, Vector3 newPosition) 
    {
        if(owner != null && requester != owner) {
            Debug.LogWarningFormat("Warning: SelectionLabelController UpdateLabelPosition() was called by {0}, while owned by {1}", requester.name, owner.name);
        }

        LabelGO.GetComponent<RectTransform>().position = newPosition;
    }

    public void UpdateLabelVisibility(GameObject requester, bool visible)
    {
        if(owner != null && requester != owner) {
            Debug.LogWarningFormat("Warning: SelectionLabelController UpdateLabelVisibility() was called by {0}, while owned by {1}", requester.name, owner.name);
        }

        LabelGO.SetActive(visible);
    }

    public void UpdateLabelText( GameObject requester, string text, Color? color = null) 
    {
        if(owner != null && requester != owner) {
            Debug.LogWarningFormat("Warning: SelectionLabelController UpdateLabelText() was called by {0}, while owned by {1}", requester.name, owner.name);
        }

        LabelGO.GetComponent<TextMeshProUGUI>().text = text;

        if(color != null) {
            LabelGO.GetComponent<TextMeshProUGUI>().color = (Color) color;
        }
    }
}
