using ItsJackAnton.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnqueueJobManager : Singleton<EnqueueJobManager>
{
    public enum ActionToExecute {  None, Dequeue, Execute }
    readonly Dictionary<string, Action> jobs = new();
    readonly Dictionary<string, ActionToExecute> executionType = new();
    readonly Queue<string> jobsToExecute = new();
    int jobsToExecuteCount;
    readonly Queue<string> jobsToDequeue = new();
    int jobsToDequeueCount;

    int frames;
    readonly int framesFreq = 18;
    public string EnqueueJob(Action job)
    {
        string jobId = HashUtil.GenID();
        jobs.Add(jobId, job);
        executionType.Add(jobId, ActionToExecute.None);

        return jobId;
    }

    /// <summary>
    /// This execute the job and dequeue it
    /// </summary>
    /// <param name="jobId"></param>
    /// <returns></returns>
    public bool ExecuteJob(string jobId)
    {
        if (jobs.ContainsKey(jobId) && executionType[jobId] ==  ActionToExecute.None)
        {
            ++jobsToExecuteCount;
            jobsToExecute.Enqueue(jobId);
            executionType[jobId] = ActionToExecute.Execute;
            return true;
        }
        return false;
    }
    /// <summary>
    /// This just dequeue the job
    /// </summary>
    /// <param name="jobId"></param>
    /// <returns></returns>
    public bool DequeueJob(string jobId)
    {
        if (jobs.ContainsKey(jobId) && executionType[jobId] == ActionToExecute.None)
        {
            ++jobsToDequeueCount;
            jobsToDequeue.Enqueue(jobId);
            executionType[jobId] = ActionToExecute.Dequeue;
            return true;
        }
        return false;
    }

    private void Update()
    {
        if(frames % framesFreq == 0)
        {
            if(jobsToExecuteCount > 0)
            {
                while(jobsToExecuteCount > 0)
                {
                    string jobId = jobsToExecute.Dequeue();

                    Action job = jobs[jobId];
                    jobs.Remove(jobId);
                    executionType.Remove(jobId);
                    job?.Invoke();
                    --jobsToExecuteCount;
                }
            }
            //
            if (jobsToDequeueCount > 0)
            {
                while (jobsToDequeueCount > 0)
                {
                    string jobId = jobsToDequeue.Dequeue();

                    jobs.Remove(jobId);
                    executionType.Remove(jobId);
                    --jobsToDequeueCount;
                }
            }
        }
        ++frames;
    }
}
