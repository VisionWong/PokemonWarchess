﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VFramework;

/// <summary>
/// 控制指定的棋子自动行动
/// </summary>
public class AutoActionController
{
    private List<EnemyChess> _enemyList;
    private List<PlayerChess> _playerList;
    private int _actionIndex = 0;

    public AutoActionController(List<EnemyChess> enemyList, List<PlayerChess> playerList)
    {
        _enemyList = enemyList;
        _playerList = playerList;
    }

    public void StartAction()
    {
        //获取离敌对方距离最短的棋子先行动
        _enemyList.Sort(new DistAscComparer(_playerList));
        MonoMgr.Instance.StartCoroutine(WaitForTurnChange());
    }

    public void NextAction()
    {
        //TODO 让上一个棋子停止
        _enemyList[_actionIndex].AI.OnActionEnd();
        MonoMgr.Instance.StartCoroutine(WaitForNextAction());
    }

    private IEnumerator WaitForTurnChange()
    {
        yield return new WaitForSeconds(1.5f);
        _enemyList[_actionIndex].AI.StartAction();
    }

    private IEnumerator WaitForNextAction()
    {
        yield return new WaitForSeconds(1f);
        _actionIndex++;
        if (_actionIndex >= _enemyList.Count)
        {
            //结束敌方行动
            MessageCenter.Instance.Broadcast(MessageType.OnEnemyTurnEnd);
            _actionIndex = 0;
        }
        else
        {
            _enemyList[_actionIndex].AI.StartAction();
        }
    }
}

public class DistAscComparer : IComparer<IChess>
{
    private List<PlayerChess> _playerList;

    public DistAscComparer(List<PlayerChess> playerList)
    {
        _playerList = playerList;
    }

    //按距离从近到远排
    public int Compare(IChess x, IChess y)
    {
        int disX = int.MaxValue;
        int disY = int.MaxValue;
        foreach (var chess in _playerList)
        {
            int curDist = Mathf.Abs(chess.StayGrid.X - x.StayGrid.X) + Mathf.Abs(chess.StayGrid.Y - x.StayGrid.Y);
            if (curDist < disX)
            {
                disX = curDist;
            }
        }
        foreach (var chess in _playerList)
        {
            int curDist = Mathf.Abs(chess.StayGrid.X - y.StayGrid.X) + Mathf.Abs(chess.StayGrid.Y - y.StayGrid.Y);
            if (curDist < disY)
            {
                disY = curDist;
            }
        }
        if (disX < disY) return -1;
        else if (disX > disY) return 1;
        return 0;
    }
}

