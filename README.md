# EnhancedScrollView
Cool "3d" scoll view for Unity3D 4.x and 5.x version

Using Unity3d's AnimationCurve to finish this EnhancedScrollView.AnimationCurve is very powerful and useful tools in developing game,the player's jumping, camera's path and so on.

## Features
1. Click left right button to recenter item
2. Click target to recenter and select item
4. Drag feature: drag enhanceScrollview to recenter item
4. Update item status with Curve horizontal time changing
5. Edit control curves for scale, position, and "depth", you can add your own curve to control item other properties

## Curves In Project
1. Position Curve. Control item's position.
2. Scale Curve. Control item's scale
3. "Depth" Curve. For determine item's back and front relationship, It's can be UIWidget's depth in NGUI(2D) or the Item's Z position value in 3D world

## In developing task
1. ~~drag feature~~ (Done)
2. animation curve editor(you can type the value for the KeyFrame)
3. ......

## How to use 

<pre><code>
YourItem: EnhanceItem
{
    // 
    // your item logic
    // click and other scrollview logic was done in EnhanceItem    
    // 
}
</code></pre>

## ScreenShot
![ScreenShot](https://github.com/tinyantstudio/EnhancedScrollView/blob/master/screenshot.png)


**If you have some cool ideas or bugs just share with us or open issues**
