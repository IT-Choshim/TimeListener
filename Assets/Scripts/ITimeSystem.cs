using System;

public interface ITimeSystem
{
    /// <summary>
    /// 添加时间回调事件
    /// </summary>
    /// <param name="destTime">Destination time.</param>
    /// <param name="callback">Callback.</param>
    /// <param name="count">count = 0表示一直循环.</param>
    int AddTimeTake(double destTime, double delayTime, Action callback = null, PETimeUnit unit = PETimeUnit.Millisecond, int count = 1);


    /// <summary>
    /// 删除时间回调事件
    /// </summary>
    /// <param name="_tid">Tid.</param>
    bool DelteTimeTake(int _tid);


    /// <summary>
    /// 替换时间回调事件
    /// </summary>
    bool ReplaceTimeTake(int tid, double destTime, double delayTime, Action callback = null, PETimeUnit unit = PETimeUnit.Millisecond, int count = 1);



    /// <summary>
    /// 添加帧回调事件
    /// </summary>
    /// <param name="destTime">Destination time.</param>
    /// <param name="callback">Callback.</param>
    /// <param name="count">count = 0表示一直循环.</param>
    int AddFrameTake(int destFrame, int delayFrame, Action callback = null, int count = 1);


    /// <summary>
    /// 删除帧回调事件
    /// </summary>
    /// <param name="_tid">Tid.</param>
    bool DelteFrameTake(int _tid);


    /// <summary>
    /// 替换帧回调事件
    /// </summary>
    bool ReplaceFrameTake(int tid, int destFrame, int delayFrame, Action callback = null, int count = 1);
}