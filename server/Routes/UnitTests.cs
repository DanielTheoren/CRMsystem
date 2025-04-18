using Xunit;
using server;
using Npgsql;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;


public class CompanyTests
{
NpgsqlDataSource db = NpgsqlDataSource.Create("Host=localhost;Database=dissatisfiedcustomer");
#region Company tests
    [Fact]
    public async Task GetAllCompanies()
    { 
        // When
        var result = await CompanyRoutes.GetCompanies(db);
        var typedResult = result.Result;
        // Then
        Assert.IsType<Ok<List<Company>>>(typedResult);
    }
    
    [Fact]
    public async Task GetACompany()
    {
        // When
        var result = await CompanyRoutes.GetCompany(2,db);
        var typedResult = result.Result;
        // Then
        Assert.IsType<Ok<Company>>(typedResult);
    }

    [Fact]
    public async Task CreateACompany()
    {
        // Given
        CompanyDTO company = new(30, "Danthe's", "010-5544662", "info@danthes.se", 3);
        // When
        var result = await CompanyRoutes.PostCompany(company, db);
        var typedResult = result.Result;
        // Then
        Assert.IsType<Created>(typedResult);
    }

    [Fact]
    public void UpdateCompany()
    {
        // Given
        CompanyDTO company = new(30, "Danthe's shop", "010-5544662", "info@danthes.se", 3);
        // When
        var result =  CompanyRoutes.PutCompany(company, db);
        var typedResult = result.Result;
        // Then
        Assert.IsType<Ok<string>>(typedResult);
   }

   [Fact]
   public async Task GetAdminList()
   {
    // When
    var result = await CompanyRoutes.GetAdmins(db);
    var typedResult = result.Result;
    // Then
    Assert.IsType<Ok<List<Admin>>>(typedResult);
   }

   [Fact]
   public async Task DeleteCompany()
   {
    // When
    var result = await CompanyRoutes.DeleteCompany(22, db);
    var typedResult = result.Result;
    // Then
    Assert.IsType<NotFound>(typedResult);
   }
#endregion

 #region User Tests

       [Fact]
    public async Task GetUsersAssignedToACompany()
    {
        // When
        var result = await UserRoutes.GetUsersFromCompanys(db);
        // Then
        Assert.IsType<List<Users>>(result);
    }

    [Fact]
    public async Task GetAllUsers()
    {
        // Then
        Assert.IsType<List<UserDTO>>(await UserRoutes.GetUsers(db));
    }

    [Fact]
    public async Task CreateUser()
    {
        // Given
        PostUserDTO user = new("Daniel Theoren", "daniel@exempel.se", "pass123", "070-3322114");
        PasswordHasher<string> hasher = new PasswordHasher<string>();
        // When
        var result = await UserRoutes.PostUser(user, db, hasher);
        var typedResult = result.Result;
        // Then
        Assert.IsType<Created<string>>(typedResult);
    }

    [Fact]
    public void UpdateUser()
    {
        // Given
        Users user = new(3, "Danne Theoren", "danne@exempel.se", "pass123", "070-3322114", 3,4);
        PasswordHasher<string> hasher = new PasswordHasher<string>();
        // When
        var result = UserRoutes.PutUsers(user, db, hasher);
        var typedResult = result.Result;
        // Then
        Assert.IsType<Ok<string>>(typedResult);
    }

    [Fact]
    public async Task DeleteAnUser()
    {
        // When
        var result = await UserRoutes.DeleteUser(26, db);
        var typedResult = result.Result;
        // Then
        Assert.IsType<NoContent>(typedResult);
    }
    #endregion

    #region Product tests

    [Fact]
    public async Task GetAllProductsFromACompany()
    {
        // When
        var result = await ProductRoute.GetProducts(1, db);
        // Then
        Assert.IsType<List<Products>>(result);
    }

    [Fact]
    public async Task GetAProduct()
    {
        // Then
        Assert.IsType<List<Products>>(await ProductRoute.GetProduct(4, db));
    }

    [Fact]
    public async Task UpdateAProduct()
    {
        // Given
        PutProductDTO product = new(20, "Laptop", "Its a damn PC");
        // When
        var result = await ProductRoute.UpdateProduct(20, product, db);
        // Then
        Assert.IsType<Ok<string>>(result);
    }

    [Fact]
    public async Task DeleteAProduct()
    {
        // Then
        //Assert.IsType<NoContent>(await ProductRoute.DeleteProduct(2, db));
    }
    #endregion

    #region Ticket tests

    [Fact]
    public async Task GetExistingTicket()
    {
        // Then
        Assert.IsType<Ticket>(await TicketRoutes.GetTicket(4, db));
    }

    [Fact]
    public async Task GetNonExistingTicket()
    {
        // Then
        Assert.IsNotType<Ticket>(await TicketRoutes.GetTicket(20000, db));
    }

    /*[Fact]
    public async Task UpdateProductInTicket()
    {
        // When
        var result = await TicketRoutes.PutTicketProduct(2, "Laptop", db);
        // Then
        Assert.IsType<Ok>(result);
    }*/

    [Fact]
    public async Task UpdateATicketsStatus()
    {
        // Then
        Assert.IsType<Ok>(await TicketRoutes.UpdateTicketStatus(2, db));
    }

    [Fact]
    public async Task GetATicketsStatus()
    {
        // Then
        Assert.IsType<List<TicketStatus>>(await TicketStatusRoutes.GetTicketStatus(db));
    }
    #endregion

    [Fact]
    public async Task SendFeedback()
    {
        // Given
        FeedbackDTO feedback = new(2,5,"not to be too lego movie but, everything id awesome!",DateTime.Now);
        // When
        var result = await FeedbackRoutes.PostFeedback(feedback, db);
        var typedResult = result.Result;
        // Then
        Assert.IsType<Created>(typedResult);

    }

}