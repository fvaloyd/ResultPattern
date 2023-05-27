# Francisvac.Result

A library to implement the result pattern.

You can find the package [here](https://www.nuget.org/packages/Francisvac.Result).

## Problem

It is very common to see the following situation:

```cs
public async Task<bool> AddProductAsync(int productId, int storeId, string userId)
{
    ProductStore? productStore = await _db.ProductStores
                                        .Include(ps => ps.Product)
                                        .FirstOrDefaultAsync(ps => ps.ProductId == productId && ps.StoreId == storeId);
   
    if (productStore is null || productStore.Quantity == 0) return false;

    Basket? userBasketExist = await _db.Baskets
                                        .FirstOrDefaultAsync(b => b.ProductId == productId && b.ApplicationUserId == userId);
    
    if (userBasketExist is not null) return false;

    Basket userBasket = new(productId, productStore.Product, userId);

    await _db.Baskets.AddAsync(userBasket);

    bool decreaseProductFromStoreResult = await _storeService.DecreaseProductAsync(productId, storeId);

    if (decreaseProductFromStoreResult is false)
    {
        _db.ChangeTracker.Clear();

        return false;
    }

    await _db.SaveChangesAsync();

    return true;
}
```

We see that the operation to add a product to the cart can return false for different reasons, but the code that consumes this method never knows why the operation returns false, and therefore cannot give the user any value feedback as to why not. the action you wanted to perform could be completed.

This would be an example of how that method would be consumed in an endpoint:

```cs
[HttpPost("AddProduct")]
public async Task<IActionResult> AddProduct(int productId, int storeId)
{
    var userId = _currentUserService.UserId;

    bool operationResult = await _basketService.AddProductAsync(productId, storeId, userId!);

    if (operationResult is false) return BadRequest("Could not add the product to the basket");

    return Ok("Product added successfully");
}
```

## Solution

One of the solutions that you can implement is to throw exceptions in the method and have the endpoint code handle them, this is a good option but there is a better one, which is to implement the Result pattern.

The purpose of the Result pattern is to return a consistent structure indicating whether the operation was successful or not, and if it was not successful, to return clear and useful error information.

Let's move on to refactor the code using the library:

```cs
public async Task<Result> AddProductAsync(int productId, int storeId, string userId)
{
    ProductStore? productStore = await _db.ProductStores
                                        .Include(ps => ps.Product)
                                        .FirstOrDefaultAsync(ps => ps.ProductId == productId && ps.StoreId == storeId);
   
    if (productStore is null || productStore.Quantity == 0) return Result.Error("No products in stock.");

    Basket? userBasketExist = await _db.Baskets
                                        .FirstOrDefaultAsync(b => b.ProductId == productId && b.ApplicationUserId == userId);
    
    if (userBasketExist is not null) return Result.Error("The cart already contains the product.");

    Basket userBasket = new(productId, productStore.Product, userId);

    await _db.Baskets.AddAsync(userBasket);

    Result decreaseProductFromStoreResult = await _storeService.DecreaseProductAsync(productId, storeId);

    if (!decreaseProductFromStoreResult.IsSuccess)
    {
        _db.ChangeTracker.Clear();

        return decreaseProductFromStoreResult;
    }

    await _db.SaveChangesAsync();

    return Result.Success("The product was added to the cart successfully.");
}
```

As you can see, we change the signature of the method, instead of returning a boolean value, we return a result. Then in each part of the code where we must return for whatever reason, whether there was an error or the operation completed successfully, we return a Result with the corresponding state and add a comment describing that state.

Now let's see what the endpoint code would look like:

```cs
[HttpPost("AddProduct")]
public async Task<IActionResult> AddProduct(int productId, int storeId)
    => await _basketService.AddProductAsync(productId, storeId, _currentUserService.UserId!).ToActionResult();
```

> note: As of version 3.0.0 you must to install `Francisvac.Result.AspNetCore` nuget package in order to use the `ToActionResult()` extension method.

We see that the endpoint code is quite clean and the most important thing is that it will return valuable information to the user in case the AddProductAsync method does not complete successfully.

If the `ToActionResult()` method catches your eye, it is simply an extension method that converts a Result to its equivalent ActionResult. For now, the only states that a Result can have are (Success, Error, and NotFound).

More statuses will be added to the Result and support for minimal APIs coming soon.

## License

[MIT license](https://github.com/Antsy15400/ResultPattern/blob/master/LICENSE.txt).