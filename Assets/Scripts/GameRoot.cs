using UnityEngine;
using UnityEngine.UI;

public class GameRoot : MonoBehaviour
{

    [SerializeField]
    private Button btn_addTime;

    [SerializeField]
    private Button btn_delteTime;

    [SerializeField]
    private Button btn_replaceTime;

    [SerializeField]
    private Button btn_addFrame;

    [SerializeField]
    private Button btn_delteFrame;

    [SerializeField]
    private Button btn_replaceFrame;

    void Start()
    {
        btn_addTime.onClick.AddListener(OnAddTimeClick);
        btn_delteTime.onClick.AddListener(OnDelteTimmeClick);
        btn_replaceTime.onClick.AddListener(OnReplaceTimeClick);
        btn_addFrame.onClick.AddListener(OnAddFrameClick);
        btn_delteFrame.onClick.AddListener(OnDelteFrameClick);
        btn_replaceFrame.onClick.AddListener(OnReplaceFrameClick);
    }

    private int tid;

    private void OnAddTimeClick()
    {
        Debug.Log("OnAddTimeClick()");
        tid = TimeSystem.Inst.AddTimeTake(2000f, 500f, OnCallback, PETimeUnit.Millisecond,0);
    }

    private void OnDelteTimmeClick()
    {
        bool isExit = TimeSystem.Inst.DelteTimeTake(tid);
        Debug.Log("DelteTime is " + isExit);
    }

    private void OnReplaceTimeClick()
    {
        bool isReplace = TimeSystem.Inst.ReplaceTimeTake(tid, 1000f, 1000f, OnCallback1, PETimeUnit.Millisecond, 3);
        Debug.Log("ReplaceTime is " + isReplace);
    }


    private void OnAddFrameClick()
    {
        Debug.Log("OnAddFrameClick()");
        tid = TimeSystem.Inst.AddFrameTake(200, 70, OnCallback, 0);
    }

    private void OnDelteFrameClick()
    {
        bool isExit = TimeSystem.Inst.DelteFrameTake(tid);
        Debug.Log("DelteFrame is " + isExit);
    }

    private void OnReplaceFrameClick()
    {
        bool isReplace = TimeSystem.Inst.ReplaceFrameTake(tid, 100, 80, OnCallback1, 3);
        Debug.Log("ReplaceFrame is " + isReplace);
    }


    private void OnCallback()
    {
        Debug.Log("OnCallback");
    }

    private void OnCallback1()
    {
        Debug.Log("OnCallback 1 --");
    }


    void Update()
    {
        TimeSystem.Inst.Update();
    }
}
