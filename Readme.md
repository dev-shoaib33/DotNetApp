# Portfolio
## Overview
This document provides detailed instructions for the modifications and additions made to the Smartwyre.DeveloperTest solution. The changes include the addition of post methods for product and rebate details, code enhancements in the getMethod and update method, implementation of Constant files for code reusability, dependency injection in the DB folder, creation of test cases, execution of the RebateService in the Console application, utilization of the CalculateRebate method, inclusion of validation checks, and implementation of exception handling mechanisms.

## Prerequisites
1. dotnet core 7 (6 probably works fine)
2. SQL Serever

## Setup
Install SQL Server
```
1. Post Methods in ProductDataStore and RebateDataStore:

- Two post methods have been added to the Smartwyre.DeveloperTest solution:
    - The first post method, located in the ProductDataStore file, handles product details.
    - The second post method, found in the RebateDataStore file, manages rebate details.

2. Code Enhancements in GetMethod and Update Method:

- The getMethod in both the RebateDataStore and ProductDataStore files has been modified to return their respective identifiers. These identifiers are required for later use in the Calculate method for accurate rebate calculation.
- Additionally, the update method in the RebateDataStore file has been updated to include the necessary code for rebate calculation.
3. Constant Files for Code Reusability:

- To promote code reusability and maintain consistency, Constant files have been introduced in the Smartwyre.DeveloperTest solution. These files contain constants that can be referenced throughout the codebase, reducing redundancy.

4. Dependency Injection in the DB Folder:

- The DB folder now includes dependency injection functionality. This implementation eliminates the need to create a new SQL connection object every time the database is accessed, opened, closed, or when commands are executed. This enhancement improves performance and efficiency.

5. Test Cases:

- Comprehensive test cases have been written for each method in the Smartwyre.DeveloperTest.Tests solution. All test cases have been executed and passed, ensuring the correctness of the implemented code.

6 Execution of RebateService in the Console Application:

- The Console application executes the RebateService from the program.cs file in the Smartwyre.DeveloperTest.Runner, as per the provided instructions. This ensures that the RebateService runs correctly and performs the required actions.

7- CalculateRebate Method:

- The CalculateRebate method calculates the rebate based on the incentive type, as per the specified requirements. It has been implemented to accurately perform rebate calculations.

8. Validation and Exception Handling:

- The solution includes comprehensive validation checks to ensure the integrity of the program. These checks prevent unexpected scenarios and ensure that the application functions correctly.
Additionally, exception handling mechanisms have been implemented throughout the codebase. In the event of an error, the application gracefully handles the exception, preventing it from breaking or crashing.

9. Utils Folder and Commons File:

- An additional folder named "utils" has been created within the project. This folder contains a cs file named "Commons." The "Commons" file includes a method named "Error," which is utilized throughout the project for sending error messages.

10. Conclusion:

- The modifications and additions made to the Smartwyre.DeveloperTest solution have enhanced its functionality, performance, and stability. The inclusion of post methods, identifier returns, constant files for code reusability, dependency injection, test cases, execution of the RebateService, validation checks, exception handling, and the introduction of the "utils" folder contribute to the overall improvement of the application. These enhancements ensure accurate calculations, prevent application breakdown, and maintain the integrity of the program.

```

User Manual: Smartwyre Rebate Calculation Console Application

1. Introduction:
The Smartwyre Rebate Calculation Console Application allows users to calculate rebates based on product and rebate details. This user manual provides step-by-step instructions for using the application effectively.

2. Starting the Application:
- Launch the Console App to begin.
- Upon starting, you will be presented with two options:
  a. Calculate Rebate
  b. Quit Application

3. Calculating Rebate:
- Select the "Calculate Rebate" option by entering the corresponding number or pressing the associated key.
- The application will prompt you to enter the necessary details for the calculation.

4. Entering Product Details:
- The application will display a message requesting you to enter the product details.
- Carefully enter the required product information, ensuring its accuracy.
- Note: The Rebate and Product Incentive Types should be the same for a successful rebate calculation.

5. Entering Rebate Details:
- After entering the product details, the application will prompt you to enter the rebate details.
- Provide the relevant rebate information as requested.
- Make sure to enter the details correctly to avoid any calculation errors.

6. Entering Volume:
- Once you have entered the rebate details, the application will ask you to input the volume.
- Enter the volume accurately to ensure accurate rebate calculation.

7. Validating and Updating the Calculation:
- If you have selected the same Incentive Type for both the product and rebate and entered all other details correctly, the application will display a message stating, "Rebate Calculation is Updated."
- This message confirms that the rebate calculation has been successfully updated based on the provided details.
- If any information is incorrect or there is an issue with the calculation, you will receive an error message: "Rebate Calculation is not updated due to an issue."

8. Quitting the Application:
- If you wish to exit the application at any time, select the "Quit Application" option by entering the corresponding number or pressing the associated key.
- The application will terminate, and you will return to the command prompt or application launcher.

9. Note:
- Please ensure that you carefully enter all the required details for both the product and rebate to avoid any calculation errors.
- Double-check that the Rebate and Product Incentive Types are the same to ensure a successful rebate calculation.
- If you encounter any issues or have questions, please refer to the provided error messages or seek assistance from the application administrator or support team.

Conclusion:
The Smartwyre Rebate Calculation Console Application allows users to calculate rebates accurately based on product and rebate details. By following the steps outlined in this user manual, you can successfully calculate rebates and obtain accurate results. If you encounter any difficulties or have further questions, please seek support from the application administrator or support team.
