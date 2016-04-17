﻿using System;
using System.Collections;
using UnityEngine;
// using System.Collections;

public class ActEnemyAttack : Act {
    private CharacterBase _target;

    public ActEnemyAttack(Enemy enemy, CharacterBase target) : base(enemy) {
        _target = target;
    }

    private IEnumerator Anim(MainSystem sys) {
       // 攻撃キャラを、ターゲットとの中間に移動させる(攻撃アニメーションの代替)
        var src = Actor.Position;
        var dst = src;
        dst.x = (src.x + _target.Position.x) / 2;
        dst.y = (src.y + _target.Position.y) / 2;
        Actor.Position = dst;

        // ターゲットの方を向く
        Actor.ChangeDir(Actor.Loc.Toward(_target.Loc));

        // ターゲットは攻撃者の方を向く。ただし既に敵の方を向いている場合は向きは変えない
        if (!sys.ExistsEnemy(_target.Front())) {
            _target.ChangeDir(_target.Loc.Toward(Actor.Loc));
        }

        var dmg = new System.Random().Next(30);
        var pos = _target.Position;
        pos.y -= 0.09f;
        yield return EffectAnim.PopupWhiteDigits(dmg, pos, () => {
            Actor.Position = src;
        });
        AnimationFinished = true;
    }

    public override void RunAnimation(MainSystem sys) {
        sys.StartCoroutine(Anim(sys));
    }

    public override void RunEffect(MainSystem sys) {
        // Actor が _target に攻撃
        DLog.D("{0} attack --> {1}", Actor, _target);
    }
}
