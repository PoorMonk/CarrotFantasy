using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBaseResourceFactory<T>
{
    T GetSingleResource(string resourcePath);	
}
