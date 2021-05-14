-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[CommentsList]
	-- Add the parameters for the stored procedure here
	@DashingId uniqueidentifier,
	@UserId uniqueidentifier,
	@CreatedDate DateTime	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	IF @CreatedDate IS NULL
		SET @CreatedDate = GETDate();

	WITH RecentComments([RecentId])
	AS
	(
	 SELECT [Id] AS [RecentId] FROM [Comment] 
	 WHERE [DashingId]=@DashingId AND [ParentId] IS NULL AND ([UserId]=@UserId OR [Active]=1) AND [CreatedDate]<=@CreatedDate
	 ORDER BY [CreatedDate] DESC OFFSET 0 ROWS FETCH FIRST 10 ROWS ONLY
	)
	SELECT [Comment].[Id],[Comment].[UserId],[Comment].[ParentId],[Comment].[Content],[Comment].[MediaUrl],[Comment].[CreatedDate],[User].FirstName + ' ' + [User].LastName AS [UserFullName],[User].imageUrl As [UserImageUrl] 
	FROM (SELECT Id from (SELECT [ParentId],[Id], ROW_NUMBER() OVER (PARTITION BY [ParentId]  ORDER BY [CreatedDate] DESC) AS [InnerCommentIndex],[Active],[UserId] FROM [Comment] 
					RIGHT JOIN RecentComments ON [RecentId] = [ParentId]) AS InnerComments where [InnerCommentIndex]<3 AND ([UserId]=@UserId OR [Active]=1)
					UNION
					SELECT [RecentId] from RecentComments) As Result 
					LEFT JOIN [Comment] ON Result.[Id] = Comment.[Id] 
					LEFT JOIN [User] ON Comment.[UserId] = [User].Id

END