CREATE PROCEDURE [dbo].[ReceiveMessage]

AS

	DECLARE @RecvReqDlgHandle uniqueidentifier
	DECLARE @RecvReqMsg nvarchar(100)
	DECLARE @RecvReqMsgName sysname

	BEGIN TRANSACTION

	WAITFOR
		(receive TOP(1)
			@RecvReqDlgHandle = CONVERSATION_HANDLE,
			@RecvReqMsg = message_body,
			@RecvReqMsgName = message_type_name
		FROM TargetQueue1DB
		);

	SELECT @RecvReqMsg AS ReceivedRequestMessage;

COMMIT
RETURN 0