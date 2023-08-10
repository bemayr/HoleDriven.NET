import { render } from "preact";
import "./style.css";
import { useEffect, useMemo, useState } from "preact/hooks";
import * as signalR from "@microsoft/signalr";
import validator from "@rjsf/validator-ajv8";
import Form from '@rjsf/fluent-ui';
import { useInterval } from "./hooks/useInterval";

export function App() {
  const queryParams = useMemo(
    () => new URLSearchParams(window.location.search),
    []
  );
  const connectionUrl = useMemo(
    () => queryParams.get("connection"),
    [queryParams]
  );
  const [guid, setGuid] = useState<string>(undefined);
  const [promptSchema, setPromptSchema] = useState<string>(undefined);
  const [connection, setConnection] =
    useState<signalR.HubConnection>(undefined);
  const [state, setState] = useState(1);

  useInterval(() => {
    if(connection.state !== signalR.HubConnectionState.Connected)
    connection.start()
  }, 10000);

  //useEffect(() => console.log(connectionUrl));

  useEffect(() => {
    const createConnection = async () => {
      if (connectionUrl) {
        const connection = new signalR.HubConnectionBuilder()
          .withUrl(connectionUrl + "hub/prompts")
          .configureLogging(signalR.LogLevel.None)
          .build();
        setConnection(connection);

        connection.on("prompt", (data) => {
          console.log(data.guid, data.schemaJson);
          setGuid(data.guid);
          setPromptSchema(JSON.parse(data.schemaJson));
        });

        try {
          await connection.start();
        } catch (error) {
          console.log({ type: "catch", error });
        }
        console.log("started");
      }
    };

    createConnection().catch((reason) =>
      console.error({ type: "err", reason })
    );
  }, [connectionUrl]);

  function submit({ formData }) {
    console.log(formData);
    connection.invoke("ProvideValue", guid, JSON.stringify(formData));
  }

  return (
    <>
      <h1>The current state is: {state}</h1>
      <div>
        {!promptSchema ? (
          <div>no prompt available</div>
        ) : (
          <Form<string>
            schema={promptSchema}
            validator={validator}
            onChange={() => console.log("changed")}
            onSubmit={submit}
            onError={() => console.log("errors")}
          />
        )}
      </div>
    </>
  );
}

render(<App />, document.getElementById("app"));
