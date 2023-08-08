import { render } from "preact";
import "./style.css";
import { useEffect, useMemo, useState } from "preact/hooks";
import * as signalR from "@microsoft/signalr";
import validator from "@rjsf/validator-ajv8";
import Form from "@rjsf/core";

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
  const [connection, setConnection] = useState<signalR.HubConnection>(undefined);

  useEffect(() => console.log(connectionUrl));

  useEffect(() => {
    if (connectionUrl) {
      let connection = new signalR.HubConnectionBuilder()
        .withUrl(connectionUrl + "/hub/prompts")
        .build();

      connection.on("prompt", (data) => {
        console.log(data.guid, data.schemaJson);
        setGuid(data.guid);
        setPromptSchema(JSON.parse(data.schemaJson));
      });

      connection.start();

      setConnection(connection)
    }
  }, [connectionUrl]);

  function submit({formData}) {
    console.log(formData)
    connection.invoke("ProvideValue", guid, JSON.stringify(formData))
  }

  return (
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
  );
}

render(<App />, document.getElementById("app"));
