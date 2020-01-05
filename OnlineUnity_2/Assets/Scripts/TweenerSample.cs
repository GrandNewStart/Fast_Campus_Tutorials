using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Holoville.HOTween;

public class TweenerSample : MonoBehaviour
{
    public string AnimString = "";
    public float AnimFloat = 0.0f;

    private void Start()
    {
        HOTween.Init(true, true, true);

        HOTween.To(this, 2.0f, new TweenParms().Prop("AnimString", "Hello World!").Loops(-1, LoopType.Yoyo));

        HOTween.To(this, 2.0f, new TweenParms().Prop("AnimFloat", 10.0f).Loops(-1, LoopType.Restart));

        //HOTween.To(transform, 4, "position", new Vector3(-3,6,0));
        /*
        HOTween.To(transform, 3, new TweenParms().Prop("position", new Vector3(0, 4, 0), true)
            .Prop("rotation", new Vector3(0, 270, 0), true)
            .Loops(-1, LoopType.Yoyo)
            .OnStepComplete(OnTweenComplete));
            */

        Sequence sequence = new Sequence(new SequenceParms().Loops(-1, LoopType.YoyoInverse));

        Tweener tweener1 = HOTween.To(transform, 1, new TweenParms().Prop("position", new Vector3(0, 4, 0), true));
        Tweener tweener2 = HOTween.To(transform, 1, new TweenParms().Prop("rotation", new Vector3(0, 270, 0), true));
        Tweener tweener3 = HOTween.To(transform, 1, new TweenParms().Prop("position", new Vector3(-4, 0, 0), true));
        Tweener tweener4 = HOTween.To(transform, 1, new TweenParms().Prop("localScale", new Vector3(1, 2, 1), true));

        sequence.Append(tweener1);
        sequence.Append(tweener2);
        sequence.Append(tweener3);
        sequence.Append(tweener4);

        Color colorTo = GetComponent<MeshRenderer>().material.color;
        colorTo.a = 0.0f;
        Tweener tweener5 = HOTween.To(GetComponent<MeshRenderer>().material, sequence.duration * 0.5f, new TweenParms().Prop("color", colorTo, true));
        sequence.Insert(sequence.duration * 0.5f, tweener5);

        sequence.Play();
    }

    void OnTweenComplete()
    {
        Debug.Log("tweening completed");
    }

    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(20, 20, 500, 100));
        GUILayout.Label("anim string : " + AnimString);
        GUILayout.Label("anim float : " + AnimFloat.ToString());
        GUILayout.EndArea();
    }
}
