// Ignore Spelling: Util

using Candid.World.Models;
using ItsJackAnton.Patterns.Broadcasts;
using ItsJackAnton.Utility;
using System.Collections.Generic;
using UnityEngine;
using static Mono.CSharp.Evaluator;

public static class UserUtil
{

    #region LEVEL
    public static (int level, int requirement, int progress, float progressPerc) GetCurrentLevel(int currentXP, float difficulty = 1.0725f)
    {
        int GetDifficultyBaseOnLevel(int level)
        {
            return Mathf.FloorToInt(1000 * Mathf.Pow(difficulty, level));
        }
        int Extract(int dna, int show_length = 1, int hide_length = 1)
        {
            return (hide_length == 0 ? dna % (int)Mathf.Pow(10, show_length) : dna % (int)Mathf.Pow(10, show_length) / (int)Mathf.Pow(10, hide_length));
        }

        int _currentXP = currentXP;
        int _xp = 0, _requirement = 0;
        int _level = 0;
        int _progress = currentXP;

        while (_xp <= _currentXP)
        {
            _requirement = GetDifficultyBaseOnLevel(_level + 1);
            int fistDigit = Extract(_requirement, 1, 0);
            if (fistDigit > 5) _requirement += 10;
            _requirement = Extract(_requirement, 11, 1) * 10;

            _xp += _requirement;
            ++_level;

            if (_xp <= _currentXP)
            {
                _progress = _currentXP - _xp;
            }
        }

        return (_level, _requirement, _progress, (float)_progress / _requirement);
    }

    #endregion


    public static bool TryGetEntityConfigData(string eid, out EntityConfig tagType)
    {
        tagType = default;

        if (BroadcastState.TryRead<DataState<WorldConfigsData>>(out var config) == false) return false;

        if (config.data.entities == null) return false;

        if (config.data.entities.TryGetValue(eid, out var val) == false) return false;

        tagType = val;

        return true;
    }
    public static bool TryGetActionConfigData(string aid, out ActionConfig tagType)
    {
        tagType = default;

        if (BroadcastState.TryRead<DataState<WorldConfigsData>>(out var config) == false) return false;

        if (config.data.actions == null) return false;

        if (config.data.actions.TryGetValue(aid, out var val) == false) return false;

        tagType = val;

        return true;
    }

    //OLD

    //GAME ITEMS
    public static bool TryGetGameItem(this UserNodeData userData, string itemId, out ItemData outValue)
    {
        outValue = default;
        return default;// return userData.items.TryGetValue(itemId, out outValue);
    }

    public static double GetGameItemQuantity(this UserNodeData userData, string itemId)
    {
        if (TryGetGameItem(userData, itemId, out var itemData))
        {
            return itemData.quantity;
        }

        return 0;
    }

    public static bool MeetsGameItemRequirements(this UserNodeData userData, List<ItemData> itemRequirements)
    {
        bool unfinished = false;
        foreach (var item in itemRequirements)
        {
            if (GetGameItemQuantity(userData, item.id) < item.quantity)
            {
                unfinished = true;
                break;
            }
        }

        return !unfinished;
    }


    public static BuffItemData GetGameBuffs(this UserNodeData userData, string itemId)
    {
        //var v = userData.buffs.Locate(e => e.Key.Contains(itemId));
        //return v.Value;
        return default;//
    }
    /// <summary>
    /// Check for Buff count, if buff configs have isTry = TRUE then the return value will be buffConfig.limit - buff count. If buff usage is specified it can be used as a filter.
    /// </summary>
    /// <param name="userData"></param>
    /// <param name="itemId"></param>
    /// <param name="buffUsage"></param>
    /// <returns></returns>
    public static double GetGameBuffQuantity(this UserNodeData userData, string itemId)
    {
        //var v = userData.buffs.Locate(e => e.Key.Contains(itemId));

        //if(v.Value != null)
        //{
        //    return v.Value.quantity;
        //}
        return 0;
    }

    /// <summary>
    /// Return the time left in seconds
    /// </summary>
    /// <param name="userData"></param>
    /// <param name="itemId"></param>
    /// <param name="buffUsage"></param>
    /// <returns></returns>
    public static long GetBuffTimeLeft(this UserNodeData userData, string itemId, string buffUsage = "")
    {
        //var buffs = GetGameBuffs(userData, itemId);
        //if (buffs == null) return 0;
        //return ((TimeUtil.NowTs() * 1000_000) - buffs.startTs) / 1000_000_000;
        return default;
    }
    public static long GetBuffProgress(this UserNodeData userData, string itemId, string buffUsage = "")
    {
        //var seconds = GetBuffTimeLeft(userData, itemId, buffUsage);

        //ConfigUtil.TryFindItem<ItemConfigTypes.BuffConfig>(itemId, out var config);

        //return seconds / config.limit;
        return default;
    }
    public static double GetAchievementCount(string achievementKey, UserNodeData userData)
    {

        //if (userData.achievements.TryGetValue(achievementKey, out var value))
        //{
        //    return value.quantity;
        //}
        return 0;
    }




    #region Balances

    public static void UpdateBalanceReq_Icp()
    {
        Broadcast.Invoke<FetchBalanceReqIcp>();
    }
    public static void UpdateBalanceReq_Rc()
    {
        Broadcast.Invoke<FetchckBalanceReqIcrc>();
    }
    public static void SetBalanceIcp(long amt)
    {
        BroadcastState.ForceInvoke<DataState<IcpData>>(e =>
        {
            e.data.amt = amt;
            e.SetAsReady();
            return e;
        });
    }
    public static void SetBalanceIcrc(string name, long amt, byte decimalCount)
    {
        BroadcastState.ForceInvoke<DataState<IcrcData>>(e =>
        {
            e.data.name = name;
            e.data.amt = amt;
            e.data.decimalCount = decimalCount;
            e.SetAsReady();
            return e;
        });
    }
    #endregion
}
