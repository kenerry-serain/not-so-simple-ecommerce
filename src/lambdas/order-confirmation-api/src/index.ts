import { APIGatewayEvent } from "aws-lambda";
import { Order } from "./entities/order";
import { AppDataSource } from "./data-source";
import { PublishCommand, SNSClient } from "@aws-sdk/client-sns";
import { env } from "./env";


export const handler = async (event: any) => {
  await AppDataSource.initialize();
  await setOrderAsConfirmed();
  await publishOrderConfirmedEvent();
  
  return {
    statusCode: 200,
    body: JSON.stringify({}),
  };

  async function publishOrderConfirmedEvent() {
    const params = {
      Message: JSON.stringify(`{\"Id\": ${event.body.Id}}`),
      TopicArn: env.TOPIC_ARN
    };
    const snsClient = new SNSClient({ region: env.REGION });
    await snsClient.send(new PublishCommand(params));
  }

  async function setOrderAsConfirmed() {
    const messageBody = JSON.parse(event.body);
    const repository = await AppDataSource.getRepository(Order);
    let order = await repository.findOneBy({ Id: messageBody.Id });

    if (order != null) {
      order.Status = messageBody.Status;
      await repository.save(order);
    }
  }
};

handler({body: "{\n    \"Id\": 1,\n    \"Status\": 1\n}"});

