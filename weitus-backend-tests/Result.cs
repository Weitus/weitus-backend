using System;
using weitus_backend;
using weitus_backend.Data.Dto;
using Xunit;

namespace weitus_backend_tests;

public class ResultTests
{
    [Fact]
    public void TestBasicSuccess()
    {
        var result = Result.Ok();
        Assert.True(result.Success);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void TestBasicError()
    {
        var result = Result.Err("test");
        Assert.False(result.Success);
        Assert.Single(result.Errors);
        Assert.Equal("test", result.Errors[0]);
    }

    [Fact]
    public void TestImplicitConversion()
    {
        var result = Result.Err("test");
        ErrorResponse errorResponse = result;
        Assert.Equal("test", errorResponse.Message);
    }

    [Fact]
    public void TestImplicitConversionSuccess()
    {
        var result = Result.Ok();
        Assert.Throws<InvalidOperationException>(() => { ErrorResponse errorResponse = result; });
    }

    [Fact]
    public void TestGenericSuccess()
    {
        var result = Result<string>.Ok("test");
        Assert.True(result.Success);
        Assert.Empty(result.Errors);
        Assert.Equal("test", result.Value);
    }

    [Fact]
    public void TestGenericError()
    {
        var result = Result<int>.Err("test");
        Assert.False(result.Success);
        Assert.Single(result.Errors);
        Assert.Equal("test", result.Errors[0]);
        Assert.Equal(default(int), result.Value);
    }

    [Fact]
    public void TestGenericImplicitConversionStruct()
    {
        var result = Result<int>.Ok(5);
        int value = result;
        Assert.Equal(5, value);
    }

    [Fact]
    public void TestGenericImplicitConversionClass()
    {
        var result = Result<string>.Ok("test");
        string value = result;
        Assert.Equal("test", value);
    }

    [Fact]
    public void TestGenericImplicitConversionError()
    {
        var result = Result<string>.Err("testerr");
        Assert.Throws<InvalidOperationException>(() => { string value = result; });
    }

    [Fact]
    public void TestGenericErrorImplicitConversionError()
    {
        var result = Result<string>.Ok("testerr");
        Assert.Throws<InvalidOperationException>(() => { ErrorResponse errorResponse = result; });
    }
}