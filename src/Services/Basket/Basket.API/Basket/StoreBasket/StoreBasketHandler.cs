﻿using Basket.API.Data;
using Basket.API.Models;
using BuildingBlocks.CQRS;
using FluentValidation;

namespace Basket.API.Basket.StoreBasket;

public record StoreBasketCommand(ShoppingCart Cart) : ICommand<StoreBasketResult>;
public record StoreBasketResult(string UserName);

public class StoreBasketCommandValidator : AbstractValidator<StoreBasketCommand>
{
    public StoreBasketCommandValidator()
    {
        RuleFor(x => x.Cart).NotNull().WithMessage("Cart can not be null");
        RuleFor(x => x.Cart.UserName).NotNull().WithMessage("UserName is required");
    }
}

public class StoreBasketCommandHandler(IBasketRepository basketRepository)
    : ICommandHandler<StoreBasketCommand, StoreBasketResult>
{
    public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
    {
        ShoppingCart cart = command.Cart;

        //TODO: store basket in database(use Marten upsert - if exist = update, if not exist insert)
        //TODO: update cache

        await basketRepository.StoreBasket(command.Cart, cancellationToken);

        return new StoreBasketResult(command.Cart.UserName);
     }
}
