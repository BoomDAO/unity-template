// Ignore Spelling: Util

using Candid.World.Models;
using ItsJackAnton.Patterns.Broadcasts;
using ItsJackAnton.Utility;
using ItsJackAnton.Values;
using Newtonsoft.Json;
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

    #region Config
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

        if (config.data.actions.TryGetValue(aid, out var val) == false)
        {
            Debug.LogError($"id {aid} doesn't exist in configs");
            return false;
        }

        tagType = val;

        return true;
    }
    #endregion

    #region Balances

    public static void UpdateBalanceReq_Icp()
    {
        Broadcast.Invoke<FetchBalanceReqIcp>();
    }
    public static void UpdateBalanceReq_Rc()
    {
        Broadcast.Invoke<FetchckBalanceReqIcrc>();
    }
    public static void UpdateTokenBalance(params Token[] token)
    {
        BroadcastState.ForceInvoke<DataState<TokensData>>(e =>
        {
            e.data = new(e.data, token);
            e.SetAsReady();
            return e;
        });
    }

    public static UResult<Token, string> GetToken(string canisterId)
    {
        if(BroadcastState.TryRead<DataState<TokensData>>(out var val) == false)
        {
            return new("TokensData could not be found");
        }

        if (val.IsReady() == false) return new("TokensData is not yet ready");

        if (val.data.tokens.TryGetValue(canisterId, out var token) == false) return new($"TokensData does not contain token of canister id: {canisterId}");
        return new(token);
    }

    public static UResult<List<Token>, string> GetTokens()
    {
        if (BroadcastState.TryRead<DataState<TokensData>>(out var val) == false)
        {
            return new("TokensData could not be found");
        }

        if (val.IsReady() == false) return new("TokensData is not yet ready");

        List<Token> tokens = new();
        val.data.tokens.Iterate(e =>
        {
            tokens.Add(e.Value);
        });
        return new(tokens);
    }

    #endregion
}
