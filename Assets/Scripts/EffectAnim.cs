﻿using UnityEngine;
// using System;
using System.Collections;
using System.Collections.Generic;

public static class EffectAnim {
    private static IEnumerator StartAnimation(string path, Vector3 pos) {
        var obj = Res.Create(path, pos);
        while (obj != null) yield return null;
    }

    public static IEnumerator PopupWhiteDigits(CharacterBase target, int n) {
        var pos = target.Position;
        pos.y -= 0.09f;
        return PopupDigits(pos, n, "Prefabs/Digits/digits_white_");
    }

    public static IEnumerator PopupGreenDigits(CharacterBase target, int n) {
        var pos = target.Position;
        pos.y -= 0.09f;
        return PopupDigits(pos, n, "Prefabs/Digits/digits_green_");
    }

    private static IEnumerator PopupDigits(Vector3 pos, int n, string pathPrefix) {
        float fontWidth = 0.14f;

        var digits = new List<GameObject>();
        var ds = Utils.Digits(n);
        float x = pos.x - fontWidth * ds.Length / 2.0f + fontWidth / 2;
        foreach (var d in ds) {
            var obj = Resources.Load(pathPrefix + d);
            var gobj = (GameObject)GameObject.Instantiate(obj, new Vector3(x, pos.y, pos.z), Quaternion.identity);
            digits.Add(gobj);
            x += fontWidth;
        }

        float v = -0.059f; // velocity
        float g = 0.008f; // gravity
        g *= 2;
        float elapsed = 0;

        int frame = 0;
        float y = pos.y;
        while (true) {
            int f = (int)(elapsed / 0.033f);
            if (frame < f) {
                frame++;
                y -= v;
                v += g;
                if (y <= pos.y) {
                    v *= -0.45f;
                    y = pos.y;

                    if (Mathf.Abs(v) < 0.016f + 0.01) {
                        v = 0;
                        foreach (var digit in digits) {
                            var p = digit.transform.position;
                            p.y = y;
                            digit.transform.position = p;
                        }
                        yield return new WaitForSeconds(0.4f);
                        foreach (var digit in digits) {
                            GameObject.Destroy(digit);
                        }
                        break;
                    }
                }

                foreach (var digit in digits) {
                    var p = digit.transform.position;
                    p.y = y;
                    digit.transform.position = p;
                }
            }
            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    public static IEnumerator Dead(Vector3 pos) {
        return StartAnimation("Prefabs/Animations/dead", pos);
    }

    public static IEnumerator Heal(Vector3 pos) {
        return StartAnimation("Prefabs/Animations/heal_heart", pos);
    }

    // TODO:rename
    public static IEnumerator Warp(Vector3 pos) {
        var obj = Resources.Load("Prefabs/Animations/warp");
        var gobj = (GameObject)GameObject.Instantiate(obj, pos, Quaternion.identity);
        var p = gobj.transform.position;
        p.y += 0.15f;
        gobj.transform.position = p;
        while (gobj != null) {
            yield return null;
        }
    }

    public static IEnumerator Aura(Vector3 pos) {
        return StartAnimation("Prefabs/Animations/aura", pos);
    }

    public static IEnumerator Aura2(Vector3 pos) {
        var obj = Resources.Load("Prefabs/Animations/aura2");
        var gobj = (GameObject)GameObject.Instantiate(obj, pos, Quaternion.identity);
        var p = gobj.transform.position;
        p.y += 0.07f;
        gobj.transform.position = p;
        while (gobj != null) {
            yield return null;
        }
    }

    public static IEnumerator Skill(Vector3 pos) {
        return StartAnimation("Prefabs/Animations/skill", pos);
    }

    public static IEnumerator Thunder(Vector3 pos) {
        return StartAnimation("Prefabs/Animations/thunder", pos);
    }

    public static IEnumerator Landmine(Vector3 pos) {
        return StartAnimation("Prefabs/Animations/landmine", pos);
    }
}
