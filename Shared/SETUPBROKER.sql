ALTER DATABASE [ServiceBroker] 
set enable_broker WITH ROLLBACK IMMEDIATE;


CREATE MESSAGE TYPE 
	[//vvdb/MSGGrp/RequestMessage_#type#]
	VALIDATION = WELL_FORMED_XML


CREATE MESSAGE TYPE 
	[//vvdb/MSGGrp/ReplyMessage_#type#]
	VALIDATION = WELL_FORMED_XML

GO

CREATE CONTRACT [//vvdb/MSGGrp/SampleContract]
	([//vvdb/MSGGrp/RequestMessage_#type#]
	SENT BY INITIATOR,
	[//vvdb/MSGGrp/ReplyMessage_#type#]
	SENT BY TARGET
	);

GO

CREATE QUEUE TargetQueue1DB_#type#;

CREATE SERVICE [//vvdb/MSGGrp/TargetService_#type#]
	ON QUEUE TargetQueue1DB_#type#
	([//vvdb/MSGGrp/SampleContract_#type#]);

CREATE QUEUE InitiatorQueue1DB_#type#;

CREATE SERVICE [//vvdb/MSGGrp/InitiatorService_#type#]
	ON QUEUE InitiatorQueue1DB_#type#;
	
