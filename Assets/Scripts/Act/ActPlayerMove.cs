﻿using UnityEngine;
using System.Collections;

public class ActPlayerMove : Act {
    private int _drow;
    private int _dcol;

    private Camera _camera;
    private float _cameraZ;
    private GameObject _minimapLayer;
    private float _elapsed;
    private Vector3 _srcPos;
    private Vector3 _dstPos;

    private bool _isFirst = true;
    private Player _player;

    public ActPlayerMove(Player player, int drow, int dcol) : base(player) {
        _drow = drow;
        _dcol = dcol;
        _player = player;

        _camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        _cameraZ = _camera.transform.position.z;
        _minimapLayer = GameObject.Find(LayerName.Minimap);

        _elapsed = 0;
        _srcPos = player.Loc.ToPosition();
        _dstPos = (player.Loc + new Loc(drow, dcol)).ToPosition();
    }

    public override bool IsMoveAct() {
        return true;
    }

    protected override IEnumerator RunAnimation(MainSystem sys) {
        Actor.ChangeDir(Utils.ToDir(_drow, _dcol));

        // TODO: 一歩一歩が連続していないので矢印が点滅してしまう
        //Actor.ShowDirection(Utils.ToDir(_drow, _dcol));

        yield return CAction.Walk(Actor, _drow, _dcol, (x, y) => {
            _player.SyncCameraPosition();
        });

        // Actor.HideDirection();
    }

    public override void Apply(MainSystem sys) {
        var nextLoc = Actor.Loc + new Loc(_drow, _dcol);
        DLog.D("{0} move {1} -> {2}", Actor, Actor.Loc, nextLoc);
        Actor.UpdateLoc(nextLoc);
    }

    public override bool IsManualUpdate() {
        return true;
    }

    public override void Update(MainSystem sys) {
        if (_isFirst) {
            Actor.ChangeDir(Utils.ToDir(_drow, _dcol));
            _isFirst = false;
        }

        _elapsed += Time.deltaTime;
        float t = _elapsed / Config.WalkDuration;
        float x = Mathf.Lerp(_srcPos.x, _dstPos.x, t);
        float y = Mathf.Lerp(_srcPos.y, _dstPos.y, t);
        Actor.Position = new Vector3(x, y, 0);

        _player.SyncCameraPosition();
        if (_elapsed >= Config.WalkDuration) {
            _animationFinished = true;
            // 位置ずれ防止
            Actor.Position = _dstPos;
            _player.SyncCameraPosition();
        }
    }
}
