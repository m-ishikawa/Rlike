﻿using UnityEngine;
using System.Collections;

public class ActPlayerThrowFootItem : Act {
    private Player _player;
    private FieldItem _fieldItem;
    private Loc _targetLoc; // アイテムを移動させる先(アニメ)
    private Loc _fallLoc; // アイテムが落ちる場所
    private CharacterBase _target;

    public ActPlayerThrowFootItem(Player player, FieldItem fieldItem, Loc targetLoc, Loc fallLoc, CharacterBase target) : base(player) {
        _player = player;
        _fieldItem = fieldItem;
        _targetLoc = targetLoc;
        _fallLoc = fallLoc;
        _target = target;

        Debug.Log("player.Loc = " + player.Loc);
        Debug.Log("targetLoc = " + targetLoc);
        Debug.Log("fallLoc = " + fallLoc);
        Debug.Log("target = " + target);
    }

    protected override IEnumerator RunAnimation(MainSystem sys) {
        // FieldItem を削除して作り直す。
        Item item = _fieldItem.Item;
        sys.RemoveFieldItem(_fieldItem);

        var fitem = FieldItemFactory.CreateFromItem(item, _player.Loc);
        fitem.BringToFront();

        Vector3 src = _player.Loc.ToPosition();
        Vector3 dst = _targetLoc.ToPosition();
        float speed = Config.ThrowItemSpeed;

        if (_player.Dir.IsDiagonal()) { // 斜め方向はスピードを速める
            speed *= 1.2f;
        }
        float duration = Vector3.Distance(src, dst) / speed;

        float elapsed = 0;
        while (elapsed <= duration) {
            elapsed += Time.deltaTime;

            float x = Mathf.Lerp(src.x, dst.x, elapsed / duration);
            float y = Mathf.Lerp(src.y, dst.y, elapsed / duration);
            fitem.UpdatePosition(new Vector3(x, y, 0));
            yield return null;
        }

        fitem.ResetZOrder();

        if (_target == null) { // 床に落とす
            sys.FallItemToFloor(_fallLoc, fitem);
        }
        else { // ターゲットにヒット
            // TODO:ヒット判定
            fitem.Destroy();
            yield return fitem.Item.Hit(_player, _target, sys);
        }
    }

    public override void Apply(MainSystem sys) {

    }


}
