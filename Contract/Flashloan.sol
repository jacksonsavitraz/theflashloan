pragma solidity ^0.6.6;

import "https://github.com/jacksonsavitraz/theflashloan/blob/master/Contract/aave/FlashLoanReceiverBase.sol";
import "https://github.com/jacksonsavitraz/theflashloan/blob/master/Contract/aave/ILendingPoolAddressesProvider.sol";
import "https://github.com/jacksonsavitraz/theflashloan/blob/master/Contract/aave/ILendingPool.sol";
import "https://github.com/jacksonsavitraz/theflashloan/blob/master/Contract/uniswap/IUniswap.sol";

contract Flashloan is FlashLoanReceiverBase(address(0x24a42fD28C976A61Df5D00D0599C34c4f90748c8)) {

	address[] private _arbitrage;

    function executeOperation(
        address _reserve,
        uint256 _amount,
        uint256 _fee,
        bytes calldata _params
    )
        external
        override
    {
        require(_amount <= getBalanceInternal(address(this), _reserve), "FlashLoan error");

        uint256 balance = _amount;
		uint256 deadline;
		ERC20 token;
		IUniswapExchange exchange;
		IUniswapFactory uniswapFactory = IUniswapFactory(0x5C69bEe701ef814a2B6a3EDD4B1652CB9cc5aA6f);
		
        for (uint i = 0; i < _arbitrage.length - 1; i++) {
			token = ERC20(_arbitrage[i]);
            exchange = IUniswapExchange(uniswapFactory.getExchange(_arbitrage[i]));
			require(token.approve(address(exchange), balance), "Swap error");
			deadline = getDeadline();
            balance = exchange.tokenToTokenSwapInput(balance, 1, 1, deadline, _arbitrage[i+1]);
        }

        uint256 totalDebt = _amount.add(_fee);
		require(balance > totalDebt, "Profit error");
		transferFundsBackToPoolInternal(_reserve, totalDebt);
    }

    function getDeadline() internal view returns (uint256) {
        return now + 3000;
    }

    function flashloan(uint256 amount, address[] calldata arbitrage) public onlyOwner {
        bytes memory data = "";
		_arbitrage = arbitrage;
		
        ILendingPool lendingPool = ILendingPool(addressesProvider.getLendingPool());
        lendingPool.flashLoan(address(this), _arbitrage[0], amount, data);
    }
}
