package com.unicef.mis.interfaces;

public interface IGenericApiCallBack<E,T> {
    void apiCallSuccessful(E identifier, T t);
    void apiCallFailed(boolean hasSpecificError, String errorMessage);
}