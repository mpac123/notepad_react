import React, { useState } from "react";
import { Container, Menu, Segment, Button } from "semantic-ui-react";
import NotesList from "./NotesList.js";
import "./App.js";
import styles from "./App.module.css";
import { Row, Col } from "antd";
import NoteEdit from "./NoteEdit.js";

const App = () => {
  const [state, setState] = useState("list");
  const [page, setPage] = useState(1);
  const [filters, setFilters] = useState({
    category: null,
    dateFrom: null,
    dateTo: null
  });
  const [selectedNote, setSelectedNote] = useState(undefined);

  const setListMode = () => {
    setState("list");
    setSelectedNote(undefined);
  };

  const setFullListMode = () => {
    setState("list");
    setSelectedNote(undefined);
    setFilters({...filters, category: null, dateFrom: null, dateTo: null})
  };


  const setCreateMode = () => {
    setState("create");
    setSelectedNote(undefined);
  };

  return (
    <Container>
      <Segment padded basic>
        <Row>
          <Col span={12}>
            <h1 className={styles.header}>Notepad</h1>
          </Col>
          <Col>
            <Button.Group
              floated="right"
              size="large"
              basic
              style={{ marginTop: "20px" }}
            >
              <Button onClick={setFullListMode}>All notes</Button>
              <Button onClick={setCreateMode}>Add new note</Button>
            </Button.Group>
          </Col>
        </Row>
      </Segment>

      {state === "list" && (
        <NotesList
          state={state}
          setState={setState}
          page={page}
          setPage={setPage}
          filters={filters}
          setFilters={setFilters}
          setSelectedNote={setSelectedNote}
        />
      )}
      {state === "edit" && (
        <NoteEdit selectedNoteID={selectedNote.noteID} setListMode={setListMode} />
      )}
      {state === "create" && <NoteEdit setListMode={setListMode} />}
    </Container>
  );
};

export default App;
