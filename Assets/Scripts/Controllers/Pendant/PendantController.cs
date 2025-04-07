using System;
using System.Threading;
using UnityEngine;

public class PendantController : MonoBehaviour
{
    private CancellationTokenSource _equimonCrystalTransitionCancellationToken = new CancellationTokenSource();
    private CancellationTokenSource[] _nimeryusCrystalsTransitionCancellationToken = new CancellationTokenSource[7];
    public MeshRenderer EquimonCrystal;
    public MeshRenderer[] NimeryusCrystals;
    public float EquimonCrystalTimeTransition;
    public float NimerysCrystalTimeTransition;

    public async void EquimonCrystalTransition()
    {
        var material = EquimonCrystal.materials[1];
        if(_equimonCrystalTransitionCancellationToken != null)
        {
            _equimonCrystalTransitionCancellationToken.Cancel();
        }
        _equimonCrystalTransitionCancellationToken = new CancellationTokenSource();

        try
        {
            var materialValue = material.GetFloat("_Transition");
            if (Mathf.Approximately(1, materialValue))
            {
                for (float elapsedTime = 0; elapsedTime <= 1; elapsedTime += Time.deltaTime / EquimonCrystalTimeTransition)
                {
                    material.SetFloat("_Transition", 1 - elapsedTime);
                    await Awaitable.NextFrameAsync(_equimonCrystalTransitionCancellationToken.Token);
                }
                material.SetFloat("_Transition", 0);
            }
            else
            {
                for (float elapsedTime = 0; elapsedTime <= 1; elapsedTime += Time.deltaTime / EquimonCrystalTimeTransition)
                {
                    material.SetFloat("_Transition", elapsedTime);
                    await Awaitable.NextFrameAsync(_equimonCrystalTransitionCancellationToken.Token);
                }
                material.SetFloat("_Transition", 1);
            }
        }
        catch(Exception ex)
        {
            if (_equimonCrystalTransitionCancellationToken.IsCancellationRequested)
            {
                Debug.Log(ex.Message);
            }
        }
        finally
        {
            _equimonCrystalTransitionCancellationToken.Dispose();
            _equimonCrystalTransitionCancellationToken = null;
        }
    }

    public async void NimeryusCrystalsTransition(int index)
    {
        var material = NimeryusCrystals[index].material;
        var token = _nimeryusCrystalsTransitionCancellationToken[index];
        if (token != null)
        {
            token.Cancel();
        }
        _nimeryusCrystalsTransitionCancellationToken[index] = new CancellationTokenSource();
        token = _nimeryusCrystalsTransitionCancellationToken[index];
        try
        {
            var materialValue = material.GetFloat("_Transition");
            if (Mathf.Approximately(1, materialValue))
            {
                for (float elapsedTime = 0; elapsedTime <= 1; elapsedTime += Time.deltaTime / EquimonCrystalTimeTransition)
                {
                    material.SetFloat("_Transition", 1 - elapsedTime);
                    await Awaitable.NextFrameAsync(token.Token);
                }
                material.SetFloat("_Transition", 0);
            }
            else
            {
                for (float elapsedTime = 0; elapsedTime <= 1; elapsedTime += Time.deltaTime / EquimonCrystalTimeTransition)
                {
                    material.SetFloat("_Transition", elapsedTime);
                    await Awaitable.NextFrameAsync(token.Token);
                }
                material.SetFloat("_Transition", 1);
            }
        }
        catch (Exception ex)
        {
            if (token.IsCancellationRequested)
            {
                Debug.Log(ex.Message);
            }
        }
        finally
        {
            token.Dispose();
            _nimeryusCrystalsTransitionCancellationToken[index] = null;
        }
    }
}
