import React, { useState } from "react";
import { padStart } from "lodash";
import { TextField } from "@mui/material";
import { FormControl, InputLabel, MenuItem, Select } from "@material-ui/core";

import { SliderPicker } from "react-color";

const CustomEventForm = ({ course }) => {
  const [customEvent, setCustomEvent] = useState({
    eventName: "",
    eventPlace: "",
    day: course.day,
    startBlock: padStart(`${course.startBlock + 6 || "07"}:00`, 5, "0"),
    length: course.length,
    colorOfBlock: "#3F51B5",
  });

  const handleChange = (e) => {
    console.log((e.target.name, e.target.value));
    setCustomEvent((customEvent) => ({
      ...customEvent,
      [e.target.name]: e.target.value,
    }));
  };

  const handleChangeComplete = (color) => {
    console.log(color);
    setCustomEvent((customEvent) => ({
      ...customEvent,
      colorOfBlock: color.hex,
    }));
  };

  return (
    <React.Fragment>
      <TextField
        placeholder="Zadajte názov udalosti *"
        name="eventName"
        value={customEvent.eventName}
        onChange={handleChange}
        margin="normal"
        fullWidth
        variant="standard"
        required
      />
      <TextField
        label="Miesto"
        placeholder="Miesto"
        name="eventPlace"
        value={customEvent.eventPlace}
        onChange={handleChange}
        margin="normal"
        variant="standard"
        fullWidth
      />
      <FormControl fullWidth required>
        <InputLabel>Deň</InputLabel>
        <Select
          name="day"
          value={customEvent.day}
          variant="standard"
          onChange={handleChange}
        >
          <MenuItem value={1}>Pondelok</MenuItem>
          <MenuItem value={2}>Utorok</MenuItem>
          <MenuItem value={3}>Streda</MenuItem>
          <MenuItem value={4}>Štvrtok</MenuItem>
          <MenuItem value={5}>Piatok</MenuItem>
        </Select>
      </FormControl>
      <TextField
        label="Začiatok"
        type="time"
        inputProps={{ step: 3600 }}
        name="startBlock"
        value={customEvent.startBlock}
        onChange={handleChange}
        margin="normal"
        fullWidth
        variant="standard"
        required
      />
      <TextField
        label="Dĺžka"
        type="number"
        InputProps={{
          inputProps: {
            min: 1,
            max: 20 - customEvent.startBlock.substring(0, 2),
          },
        }}
        name="length"
        value={customEvent.length}
        onChange={handleChange}
        margin="normal"
        fullWidth
        variant="standard"
        required
      />
      <TextField
        label="Farba bloku"
        type="text"
        inputProps={{ step: 3600 }}
        name="colorOfBlock"
        value={customEvent.colorOfBlock}
        onChange={handleChange}
        margin="normal"
        fullWidth
        variant="standard"
        required
      />
      <SliderPicker
        color={customEvent.colorOfBlock}
        onChangeComplete={handleChangeComplete}
      />{" "}
    </React.Fragment>
  );
};

export default CustomEventForm;
